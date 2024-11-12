using SavingsInvestment.Models.DTO;
using SavingsInvestment.Models;

namespace SavingsInvestment.Services
{
    public interface ISavingsInvestmentService
    {
        Task<ServiceResponse<Investment>> AddInvestmentAsync(Investment investment);
        Task<ServiceResponse<Investment>> UpdateInvestmentAsync(Investment investment);
        Task<ServiceResponse<PortfolioSummaryDTO>> GetPortfolioSummaryAsync(string userId);
        Task<ServiceResponse<InvestmentSummaryDTO>> GetInvestmentsByTypeAsync(string userId, string type);
        Task<ServiceResponse<SavingsGoal>> SetSavingsGoalAsync(SavingsGoal goal);
        Task<ServiceResponse<List<SavingsGoal>>> GetSavingsGoalsAsync(string userId);
        Task<ServiceResponse<bool>> UpdateInvestmentValuesAsync(string userId);
    }
}
