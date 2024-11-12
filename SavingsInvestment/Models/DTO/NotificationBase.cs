namespace SavingsInvestment.Models.DTO
{
    
    public abstract class NotificationBase
    {
        public string UserId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Priority { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
