namespace ExpenseBudget.Models.DTO
{
    public class ExpenseSummaryDTO
    {
        public string Category { get; set; }
        public decimal TotalSpent { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal RemainingBudget { get; set; }
        public decimal SpendingPercentage { get; set; }
    }
}
