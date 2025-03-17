using BusinessLogicLayer.DTOs;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;

        public OrdersController(IOrderRepo orderRepo)
        {
            _orderRepo = orderRepo;
        }
        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllOrders();
            var totalOrders = orders.Count();
            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var pendingOrders = orders.Count(o => o.Status == "Pending");
            var completedOrders = orders.Count(o => o.Status == "Completed");
            var canceledOrders = orders.Count(o => o.Status == "Canceled");
            var recentOrders = orders.OrderByDescending(o => o.OrderDate)
                         .Take(5)
                         .Select(o => new OrderDto
                         {
                             OrderId = o.OrderId,
                             OrderDate = o.OrderDate,
                             TotalAmount = o.TotalAmount,
                             userName = o.User.UserName,
                             Status = o.Status
                         })
                         .ToList();

            var model = new OrderDashboardDto
            {
                TotalOrders = totalOrders,
                TotalRevenue = totalRevenue,
                PendingOrders = pendingOrders,
                CompletedOrders = completedOrders,
                CanceledOrders = canceledOrders,
                RecentOrders = recentOrders,
            };
            return View(model);
        }
    }
}
