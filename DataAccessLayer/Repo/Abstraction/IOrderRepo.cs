

using DataAccessLayer.Enitites;

namespace DataAccessLayer.Repo.Abstraction
{
    public interface IOrderRepo
    {

        Task AddOrder(Order order);
        Task UpdateOrder(Order order);
        Task DeleteOrder(int orderId);

        Task<Order> GetOrderById(int orderId);

        Task<IEnumerable<Order>> GetAllOrders();
    }
}
