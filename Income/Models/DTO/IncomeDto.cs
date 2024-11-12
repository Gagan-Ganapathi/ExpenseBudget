namespace IncomeMicroservice.Models.DTO
{
    public class IncomeDto
    {
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
    }

    public class MonthlyIncomeDto1
    {
        public string UserId { get; set; }
        public decimal TotalMonthlyIncome { get; set; }
        public DateTime Month { get; set; }
    }
}
