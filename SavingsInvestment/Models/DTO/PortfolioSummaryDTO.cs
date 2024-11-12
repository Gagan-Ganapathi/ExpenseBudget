namespace SavingsInvestment.Models.DTO
{
    public class PortfolioSummaryDTO
    {
        public decimal TotalInvestments { get; set; }
        public decimal TotalCurrentValue { get; set; }
        public decimal TotalReturns { get; set; }
        public decimal OverallReturnPercentage { get; set; }
        public Dictionary<string, decimal> AllocationPercentages { get; set; }
        public List<InvestmentSummaryDTO> InvestmentsByType { get; set; }

    }
}
