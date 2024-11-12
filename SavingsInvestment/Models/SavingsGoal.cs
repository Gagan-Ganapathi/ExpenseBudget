namespace SavingsInvestment.Models
{
    public class SavingsGoal
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string GoalName { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public DateTime TargetDate { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; }
    }
}
