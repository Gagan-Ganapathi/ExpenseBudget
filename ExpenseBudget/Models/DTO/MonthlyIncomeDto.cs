namespace ExpenseBudget.Models.DTO
{
    public class MonthlyIncomeDto
    {
        public string UserId { get; set; }
        public decimal TotalMonthlyIncome { get; set; }
        public DateTime Month { get; set; }
    }
}
