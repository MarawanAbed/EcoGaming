

using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface IOrderServices
    {
        Task AddOrder(OrderDto order);
        Task<IEnumerable<OrderDto>> GetAllOrders();
        Task<OrderDto> GetOrder(int orderId);
        Task UpdateOrder(OrderDto order);
        Task<OrderDto> GetOrderById(string userId);
        Task RemoveOrder(int orderId);
        Task<List<OrderDto>> GetOrdersByUserId(string userId);

    }
}
