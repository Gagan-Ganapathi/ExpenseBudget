namespace SavingsInvestment.Services.Interface
{
    public interface INotificationService
    {
       Task SendInvestmentAlert(string userId, string investmentType, decimal returnPercentage);
        Task SendGoalCompletionNotification(string userId, string goalName);
    }

}
