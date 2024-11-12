using ExpenseBudget.Models;
using ExpenseBudget.Models.DTO;

namespace SavingsInvestment.Services.Interface
{
    public interface IExpenseBudgetService
    {

        Task<IEnumerable<Expense>> GetUserExpensesAsync(string userId);
        Task<IEnumerable<Budget>> GetUserBudgetsAsync(string userId);
        Task<ExpenseSummaryDTO> GetCategoryAnalysisAsync(string userId, string category);
        Task<Expense> AddExpenseAsync(Expense expense);
        Task<Budget> SetBudgetAsync(Budget budget);
        Task<IEnumerable<ExpenseSummaryDTO>> GetMonthlySummaryAsync(string userId);
        Task UpdateInvestmentAllocation(string userId, decimal amount);
    }
}

