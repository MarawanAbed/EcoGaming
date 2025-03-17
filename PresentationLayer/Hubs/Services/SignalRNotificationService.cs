using BusinessLogicLayer.Services.Abstraction;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Hubs.Services
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<CartHub> _cartHub;

        public SignalRNotificationService(IHubContext<CartHub> cartHub)
        {
            _cartHub = cartHub ?? throw new ArgumentNullException(nameof(cartHub)); // ✅ Correct!
        }

        public async Task NotifyCartUpdated(string userId)
        {
            await _cartHub.Clients.User(userId).SendAsync("ReceiveCartUpdate");
        }
        //Now, SignalR only exists in Presentation Layer!
        //BLL stays clean and unaware of SignalR.
    }
}
