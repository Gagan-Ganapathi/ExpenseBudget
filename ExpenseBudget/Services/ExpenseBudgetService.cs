using ExpenseBudget.Data;
using ExpenseBudget.Models.DTO;
using ExpenseBudget.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;


namespace ExpenseBudget.Services
{
    public class ExpenseBudgetService : IExpenseBudgetService
    {
        private readonly ExpenseBudgetContext _context;
        private readonly ILogger<ExpenseBudgetService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ExpenseBudgetService(
            ExpenseBudgetContext context,
            ILogger<ExpenseBudgetService> logger,
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Expense>> GetUserExpensesAsync(string userId)
        {
            return await _context.Expenses
                .Where(e => e.UserId == userId)
                .OrderByDescending(e => e.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Budget>> GetUserBudgetsAsync(string userId)
        {
            return await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<ExpenseSummaryDTO> GetCategoryAnalysisAsync(string userId, string category)
        {
            var budget = await _context.Budgets
                .FirstOrDefaultAsync(b => b.UserId == userId && b.Category == category);

            var totalSpent = await _context.Expenses
                .Where(e => e.UserId == userId &&
                           e.Category == category &&
                           e.Date.Month == DateTime.Now.Month)
                .SumAsync(e => e.Amount);

            var budgetAmount = budget?.Amount ?? 0;
            var remainingBudget = budgetAmount - totalSpent;
            var spendingPercentage = budgetAmount > 0 ? (totalSpent / budgetAmount) * 100 : 0;

            return new ExpenseSummaryDTO
            {
                Category = category,
                TotalSpent = totalSpent,
                BudgetAmount = budgetAmount,
                RemainingBudget = remainingBudget,
                SpendingPercentage = spendingPercentage
            };
        }

        public async Task<Expense> AddExpenseAsync(Expense expense)
        {
            try
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();
                return expense;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding expense for user {UserId}", expense.UserId);
                throw;
            }
        }

        //public async Task<Budget> SetBudgetAsync(Budget budget)
        // { 
        //     try
        //     {
        //         var existingBudget = await _context.Budgets.FirstOrDefaultAsync(b => b.UserId == budget.UserId && b.Category == budget.Category);

        //         if (existingBudget != null)
        //         {
        //             existingBudget.Amount = budget.Amount;
        //             existingBudget.StartDate = budget.StartDate;
        //             existingBudget.EndDate = budget.EndDate;
        //             existingBudget.IsRecurring = budget.IsRecurring;
        //             existingBudget.Period = budget.Period;
        //         }
        //         else
        //         {
        //             _context.Budgets.Add(budget);
        //         }

        //         await _context.SaveChangesAsync();
        //         return budget;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error setting budget for user {UserId}", budget.UserId);
        //         throw;
        //     }
        // }


        public async Task<Budget> SetBudgetAsync(Budget budget)
        {
            try
            {
                if (!await ValidateTotalBudgetAgainstIncome(
                    budget.UserId,
                    budget.Amount))
                {
                    throw new InvalidOperationException(
                        "Total budget cannot exceed monthly income");
                }

                var existingBudget = await _context.Budgets
                    .FirstOrDefaultAsync(b =>
                        b.UserId == budget.UserId &&
                        b.Category == budget.Category);

                if (existingBudget != null)
                {
                    existingBudget.Amount = budget.Amount;
                    existingBudget.StartDate = budget.StartDate;
                    existingBudget.EndDate = budget.EndDate;
                    existingBudget.IsRecurring = budget.IsRecurring;
                    existingBudget.Period = budget.Period;
                }
                else
                {
                    _context.Budgets.Add(budget);
                }

                await _context.SaveChangesAsync();
                return budget;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error setting budget for user {UserId}",
                    budget.UserId);
                throw;
            }
        }

        public async Task<IEnumerable<ExpenseSummaryDTO>> GetMonthlySummaryAsync(string userId)
        {
            var categories = await _context.Budgets
                .Where(b => b.UserId == userId)
                .Select(b => b.Category)
                .Distinct()
                .ToListAsync();

            var summaries = new List<ExpenseSummaryDTO>();
            foreach (var category in categories)
            {
                summaries.Add(await GetCategoryAnalysisAsync(userId, category));
            }

            return summaries;
        }

        public async Task<bool> ValidateTotalBudgetAgainstIncome(
       string userId,
       decimal newBudgetAmount)
        {
            var existingBudgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            var totalExistingBudget = existingBudgets.Sum(b => b.Amount);
            var totalBudget = totalExistingBudget + newBudgetAmount;

            var incomeApiUrl =
                _configuration["ServiceUrls:Income"] +
                $"/api/income/monthly/{userId}";
            var response = await _httpClient.GetAsync(incomeApiUrl);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Failed to fetch income data for user {UserId}",
                    userId);
                return false;
            }
            var incomeData = await response.Content.ReadFromJsonAsync<MonthlyIncomeDto>();
            if (incomeData == null)
            {
                _logger.LogError(
                    "Failed to deserialize income data for user {UserId}",
                    userId);
                return false;
            }
            return totalBudget <= incomeData.TotalMonthlyIncome;

        }

        public async Task UpdateMonthlyIncome(string userId, decimal monthlyIncome)
        {
            var budgets = await _context.Budgets
                .Where(b => b.UserId == userId)
                .ToListAsync();

            foreach (var budget in budgets)
            {
                budget.MonthlyIncome = monthlyIncome;
            }

            await _context.SaveChangesAsync();
        }

       
    }
}

