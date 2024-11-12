namespace SavingsInvestment.Models
{
    public class InvestmentDetails
    {
        public int Id { get; set; }
        public string Symbol { get; set; }  // For stocks/mutual funds
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Returns { get; set; }
        public decimal ReturnPercentage { get; set; }
        public int InvestmentId { get; set; }

        public Investment Investment { get; set; }
    }
}
