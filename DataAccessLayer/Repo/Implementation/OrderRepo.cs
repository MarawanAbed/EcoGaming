
using DataAccessLayer.Database;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repo.Implementation
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddOrder(Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .Include(x => x.User).ToListAsync();
        }

        public Task<Order> GetOrderById(string userId)
        {
            return _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<Order>> GetOrdersByUserId(string userId)
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .ThenInclude(od => od.Product)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        }

        public async Task UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
    }
}  