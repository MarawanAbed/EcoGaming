

using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface IOrderServices
    {
        Task AddOrder(OrderDto order);
        Task<IEnumerable<OrderDto>> GetOrders(string userId);
        Task<OrderDto> GetOrder(int orderId);
        Task UpdateOrder(OrderDto order);
        Task RemoveOrder(int orderId);
    }
}
