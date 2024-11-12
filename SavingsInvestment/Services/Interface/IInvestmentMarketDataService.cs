using SavingsInvestment.Models;

namespace SavingsInvestment.Services.Interface
{
    public interface IInvestmentMarketDataService
    {
        Task<ServiceResponse<decimal>> GetCurrentPrice(string symbol, string investmentType);
        Task<ServiceResponse<Dictionary<string, decimal>>> GetBatchPrices(List<string> symbols);
    }

}
