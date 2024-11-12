using ExpenseBudget.Models.DTO;
using ExpenseBudget.Models;

namespace ExpenseBudget.Services
{
    public interface IExpenseBudgetService
    {
        Task<IEnumerable<Expense>> GetUserExpensesAsync(string userId);
        Task<IEnumerable<Budget>> GetUserBudgetsAsync(string userId);
        Task<ExpenseSummaryDTO> GetCategoryAnalysisAsync(string userId, string category);
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Budget> SetBudgetAsync(Budget budget);
        Task<IEnumerable<ExpenseSummaryDTO>> GetMonthlySummaryAsync(string userId);
        Task<bool> ValidateTotalBudgetAgainstIncome(string userId, decimal totalBudget);
        Task UpdateMonthlyIncome(string userId, decimal monthlyIncome);
    }
}
