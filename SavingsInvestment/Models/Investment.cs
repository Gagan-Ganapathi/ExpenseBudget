namespace SavingsInvestment.Models
{
    public class Investment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string InvestmentType { get; set; }  // Stocks, MutualFunds, Gold, etc.
        public decimal Amount { get; set; }
        public decimal CurrentValue { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int InvestmentDetailsId { get; set; }

        public InvestmentDetails Details { get; set; }
    }
}
