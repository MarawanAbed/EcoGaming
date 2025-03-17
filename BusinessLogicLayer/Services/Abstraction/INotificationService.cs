

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface INotificationService
    {
        Task NotifyCartUpdated(string userId);
    }

}
