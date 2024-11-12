namespace SavingsInvestment.Models.DTO
{
    public class MarketDataResponse
    {
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
        public string Currency { get; set; }
    }
}
