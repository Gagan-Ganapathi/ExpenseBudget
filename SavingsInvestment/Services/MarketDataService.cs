using SavingsInvestment.Models;
using SavingsInvestment.Models.DTO;
using SavingsInvestment.Services.Interface;

namespace SavingsInvestment.Services
{
    //public class MarketDataService
    //{
    //}
    public class InvestmentMarketDataService : IInvestmentMarketDataService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<InvestmentMarketDataService> _logger;
        private readonly IConfiguration _configuration;

        public InvestmentMarketDataService(
            HttpClient httpClient,
            ILogger<InvestmentMarketDataService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["MarketDataApi:BaseUrl"]);
        }

        public async Task<ServiceResponse<decimal>> GetCurrentPrice(string symbol, string investmentType)
        {
            try
            {
                var endpoint = investmentType.ToLower() switch
                {
                    "stocks" => $"/api/stocks/{symbol}/price",
                    "mutualfunds" => $"/api/funds/{symbol}/nav",
                    "gold" => "/api/commodities/gold/price",
                    _ => throw new ArgumentException($"Unsupported investment type: {investmentType}")
                };

                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<MarketDataResponse>();
                return ServiceResponse<decimal>.SuccessResponse(result.Price);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching price for {Symbol} ({Type})", symbol, investmentType);
                return ServiceResponse<decimal>.ErrorResponse(
                    "Error fetching market data",
                    ex.Message);
            }
        }

        public async Task<ServiceResponse<Dictionary<string, decimal>>> GetBatchPrices(List<string> symbols)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/prices/batch", symbols);
                response.EnsureSuccessStatusCode();

                var result = await response.Content
                    .ReadFromJsonAsync<Dictionary<string, decimal>>();
                return ServiceResponse<Dictionary<string, decimal>>.SuccessResponse(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching batch prices for {Count} symbols", symbols.Count);
                return ServiceResponse<Dictionary<string, decimal>>.ErrorResponse(
                    "Error fetching batch prices",
                    ex.Message);
            }
        }
    }
}
