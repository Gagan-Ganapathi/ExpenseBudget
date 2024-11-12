namespace ExpenseBudget.Models
{
    public class Budget
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public required string Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsRecurring { get; set; }
        public string Period { get; set; }
        public decimal? MonthlyIncome { get; set; } // New property

    }
}
