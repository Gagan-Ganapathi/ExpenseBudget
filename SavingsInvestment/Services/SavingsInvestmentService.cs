using SavingsInvestment.Data;
using SavingsInvestment.Models.DTO;
using SavingsInvestment.Models;
using Microsoft.EntityFrameworkCore;
//using ExpenseBudget.Services;
using SavingsInvestment.Services.Interface;

namespace SavingsInvestment.Services
{
    public class SavingsInvestmentService : ISavingsInvestmentService
    {

        private readonly SavingsInvestmentContext _context;
        private readonly ILogger<SavingsInvestmentService> _logger;
        private readonly IExpenseBudgetService _expenseBudgetService;
        private readonly IInvestmentMarketDataService _marketDataService;
        private readonly INotificationService _notificationService;

        public SavingsInvestmentService(
            SavingsInvestmentContext context,
            ILogger<SavingsInvestmentService> logger,
            IExpenseBudgetService expenseBudgetService,
            IInvestmentMarketDataService marketDataService,
            INotificationService notificationService)
        {
            _context = context;
            _logger = logger;
            _expenseBudgetService = expenseBudgetService;
            _marketDataService = marketDataService;
            _notificationService = notificationService;
        }

        public async Task<ServiceResponse<Investment>> AddInvestmentAsync(Investment investment)
        {
            try
            {
                // Calculate current value based on market data
                var marketData = await _marketDataService.GetCurrentPrice(
                    investment.Details.Symbol,
                    investment.InvestmentType);

                if (marketData.Success)
                {
                    investment.Details.CurrentPrice = marketData.Data;
                    investment.Details.CurrentPrice = marketData.Data * investment.Details.Quantity;
                    investment.Details.Returns = investment.CurrentValue - investment.Amount;
                    investment.Details.ReturnPercentage =
                        (investment.Details.Returns / investment.Amount) * 100;
                }

                await _context.Investments.AddAsync(investment);
                await _context.SaveChangesAsync();

                // Update budget allocation
                await _expenseBudgetService.UpdateInvestmentAllocation(
                    investment.UserId,
                    investment.Amount);

                return ServiceResponse<Investment>.SuccessResponse(investment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding investment for user {UserId}", investment.UserId);
                return ServiceResponse<Investment>.ErrorResponse(
                    "Error adding investment",
                    ex.Message);
            }
        }

        public async Task<ServiceResponse<PortfolioSummaryDTO>> GetPortfolioSummaryAsync(string userId)
        {
            try
            {
                var investments = await _context.Investments
                    .Include(i => i.Details)
                    .Where(i => i.UserId == userId && i.IsActive)
                    .ToListAsync();

                var summary = new PortfolioSummaryDTO
                {
                    TotalInvestments = investments.Sum(i => i.Amount),
                    TotalCurrentValue = investments.Sum(i => i.CurrentValue),
                    InvestmentsByType = await GetInvestmentSummaryByType(investments)
                };

                summary.TotalReturns = summary.TotalCurrentValue - summary.TotalInvestments;
                summary.OverallReturnPercentage =
                    (summary.TotalReturns / summary.TotalInvestments) * 100;

                // Calculate allocation percentages
                summary.AllocationPercentages = investments
                    .GroupBy(i => i.InvestmentType)
                    .ToDictionary(
                        g => g.Key,
                        g => (g.Sum(i => i.CurrentValue) / summary.TotalCurrentValue) * 100);

                return ServiceResponse<PortfolioSummaryDTO>.SuccessResponse(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting portfolio summary for user {UserId}", userId);
                return ServiceResponse<PortfolioSummaryDTO>.ErrorResponse(
                    "Error retrieving portfolio summary",
                    ex.Message);
            }
        }

        private async Task<List<InvestmentSummaryDTO>> GetInvestmentSummaryByType(
            List<Investment> investments)
        {
            return  investments
                .GroupBy(i => i.InvestmentType)
                .Select(g => new InvestmentSummaryDTO
                {
                    InvestmentType = g.Key,
                    TotalInvested = g.Sum(i => i.Amount),
                    CurrentValue = g.Sum(i => i.CurrentValue),
                    Returns = g.Sum(i => i.Details.Returns),
                    ReturnPercentage =
                        (g.Sum(i => i.Details.Returns) / g.Sum(i => i.Amount)) * 100,
                    Investments = g.ToList()
                })
                .ToList();
        }

        public async Task<ServiceResponse<bool>> UpdateInvestmentValuesAsync(string userId)
        {
            try
            {
                var investments = await _context.Investments
                    .Include(i => i.Details)
                    .Where(i => i.UserId == userId && i.IsActive)
                    .ToListAsync();

                foreach (var investment in investments)
                {
                    var marketData = await _marketDataService.GetCurrentPrice(
                        investment.Details.Symbol,
                        investment.InvestmentType);

                    if (marketData.Success)
                    {
                        investment.Details.CurrentPrice = marketData.Data;
                        investment.CurrentValue = marketData.Data * investment.Details.Quantity;
                        investment.Details.Returns = investment.CurrentValue - investment.Amount;
                        investment.Details.ReturnPercentage =
                            (investment.Details.Returns / investment.Amount) * 100;

                        // Check if return crosses threshold for notification
                        if (Math.Abs(investment.Details.ReturnPercentage) >= 10)
                        {
                            await _notificationService.SendInvestmentAlert(
                                userId,
                                investment.InvestmentType,
                                investment.Details.ReturnPercentage);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                return ServiceResponse<bool>.SuccessResponse(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating investment values for user {UserId}", userId);
                return ServiceResponse<bool>.ErrorResponse(
                    "Error updating investment values",
                    ex.Message);
            }
        }

        public async Task<ServiceResponse<SavingsGoal>> SetSavingsGoalAsync(SavingsGoal goal)
        {
            try
            {
                var existingGoal = await _context.SavingsGoals
                    .FirstOrDefaultAsync(g => g.Id == goal.Id);

                if (existingGoal != null)
                {
                    existingGoal.TargetAmount = goal.TargetAmount;
                    existingGoal.CurrentAmount = goal.CurrentAmount;
                    existingGoal.TargetDate = goal.TargetDate;
                    existingGoal.Priority = goal.Priority;
                    _context.SavingsGoals.Update(existingGoal);
                }
                else
                {
                    await _context.SavingsGoals.AddAsync(goal);
                }

                await _context.SaveChangesAsync();

                // Check if goal is completed
                if (goal.CurrentAmount >= goal.TargetAmount)
                {
                    goal.IsCompleted = true;
                    await _notificationService.SendGoalCompletionNotification(
                        goal.UserId,
                        goal.GoalName);
                }

                return ServiceResponse<SavingsGoal>.SuccessResponse(goal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting savings goal for user {UserId}", goal.UserId);
                return ServiceResponse<SavingsGoal>.ErrorResponse(
                    "Error setting savings goal",
                    ex.Message);
            }
        }

        public Task<ServiceResponse<Investment>> UpdateInvestmentAsync(Investment investment)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<InvestmentSummaryDTO>> GetInvestmentsByTypeAsync(string userId, string type)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<SavingsGoal>>> GetSavingsGoalsAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
