using ExpenseBudget.Models.DTO;
using ExpenseBudget.Models;
using ExpenseBudget.Services;

namespace SavingsInvestment.Services
{
    public class ExpenseBudgetService : IExpenseBudgetService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExpenseBudgetService> _logger;
        private readonly IConfiguration _configuration;

        public ExpenseBudgetService(
            HttpClient httpClient,
            ILogger<ExpenseBudgetService> logger,
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

        public async Task<ExpenseSummaryDTO> GetCategoryAnalysisAsync(string userId, string category)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/expensebudget/analysis/{userId}/{category}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ExpenseSummaryDTO>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching category analysis for user {UserId} category {Category}",
                    userId, category);
                throw;
            }
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/expensebudget/expenses", expense);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Expense>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding expense for user {UserId}", expense.UserId);
                throw;
            }
        }

        public async Task<Budget> SetBudgetAsync(Budget budget)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/expensebudget/budgets", budget);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Budget>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting budget for user {UserId}", budget.UserId);
                throw;
            }
        }

        public async Task<IEnumerable<ExpenseSummaryDTO>> GetMonthlySummaryAsync(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/expensebudget/summary/{userId}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseSummaryDTO>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching monthly summary for user {UserId}", userId);
                throw;
            }
        }

        public async Task UpdateInvestmentAllocation(string userId, decimal amount)
        {
            try
            {
                var expense = new Expense
                {
                    UserId = userId,
                    Category = "Investments",
                    Amount = amount,
                    Description = "Investment Allocation",
                    Date = DateTime.UtcNow,
                    PaymentMethod = "Investment Transfer"
                };

                var response = await _httpClient.PostAsJsonAsync("/api/expensebudget/expenses", expense);
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment allocation for user {UserId}", userId);
                throw;
            }
        }
    }
}
