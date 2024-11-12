namespace SavingsInvestment.Models.DTO
{
    public class InvestmentSummaryDTO
    {
        public string InvestmentType { get; set; }
        public decimal TotalInvested { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal Returns { get; set; }
        public decimal ReturnPercentage { get; set; }
        public List<Investment> Investments { get; set; }
    }
}
