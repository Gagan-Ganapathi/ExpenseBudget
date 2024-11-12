using ExpenseBudget.Models;
using ExpenseBudget.Models.DTO;
using SavingsInvestment.Models.DTO;
using SavingsInvestment.Services.Interface;

namespace SavingsInvestment.Services
{
    public class NotificationService : INotificationService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _configuration;

        public NotificationService(
            HttpClient httpClient,
            ILogger<NotificationService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["NotificationService:BaseUrl"]);
        }

        public async Task SendInvestmentAlert(
            string userId,
            string investmentType,
            decimal returnPercentage)
        {
            try
            {
                var notification = new InvestmentAlertNotification
                {
                    UserId = userId,
                    Type = "INVESTMENT_ALERT",
                    Message = $"Your {investmentType} investment has {(returnPercentage > 0 ? "gained" : "lost")} {Math.Abs(returnPercentage):N2}%",
                    Priority = Math.Abs(returnPercentage) >= 20 ? "HIGH" : "MEDIUM"
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "/api/notifications",
                    notification);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending investment alert for user {UserId}", userId);
            }
        }

        public async Task SendGoalCompletionNotification(string userId, string goalName)
        {
            try
            {
                var notification = new GoalCompletionNotification
                {
                    UserId = userId,
                    Type = "GOAL_COMPLETION",
                    Message = $"Congratulations! You've achieved your savings goal: {goalName}",
                    Priority = "HIGH"
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "/api/notifications",
                    notification);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending goal completion notification for user {UserId}", userId);
            }
        }
    }

    // Services/ExpenseBudgetServiceClient.cs
    public class ExpenseBudgetServiceClient : IExpenseBudgetService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExpenseBudgetServiceClient> _logger;
        private readonly IConfiguration _configuration;

        public ExpenseBudgetServiceClient(
            HttpClient httpClient,
            ILogger<ExpenseBudgetServiceClient> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ExpenseBudgetService:BaseUrl"]);
        }

        public async Task<IEnumerable<Expense>> GetUserExpensesAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/expensebudget/expenses/{userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Expense>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching expenses for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Budget>> GetUserBudgetsAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/expensebudget/budgets/{userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<Budget>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching budgets for user {UserId}", userId);
                throw;
            }
        }
        public async Task UpdateInvestmentAllocation(string userId, decimal amount)
        {
            try
            {
                var updateRequest = new
                {
                    UserId = userId,
                    Category = "Investments",
                    Amount = amount,
                    Date = DateTime.UtcNow,
                    Description = "Investment Allocation"
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "/api/expensebudget/expenses",
                    updateRequest);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment allocation for user {UserId}", userId);
                throw;
            }
        }

        public Task<ExpenseSummaryDTO> GetCategoryAnalysisAsync(string userId, string category)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> AddExpenseAsync(Expense expense)
        {
            throw new NotImplementedException();
        }

        public Task<Budget> SetBudgetAsync(Budget budget)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ExpenseSummaryDTO>> GetMonthlySummaryAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
