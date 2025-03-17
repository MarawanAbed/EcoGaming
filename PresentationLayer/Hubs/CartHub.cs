using Microsoft.AspNetCore.SignalR;

namespace PresentationLayer.Hubs
{

    public class CartHub : Hub
    {
        public async Task UpdateCart(string userId)
        {
            await Clients.User(userId).SendAsync("ReceiveCartUpdate");
        }
    }
}
