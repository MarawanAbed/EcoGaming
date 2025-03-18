using BusinessLogicLayer.DTOs;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PresentationLayer.ActionRequests.orders;

namespace PresentationLayer.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderRepo _orderRepo;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderRepo orderRepo, ILogger<OrdersController> logger)
        {
            _orderRepo = orderRepo;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderRepo.GetAllOrders();
            var totalOrders = orders.Count();
            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var pendingOrders = orders.Count(o => o.OrderStatus == "Pending");
            var completedOrders = orders.Count(o => o.OrderStatus == "Completed");
            var canceledOrders = orders.Count(o => o.OrderStatus == "Canceled");
            var recentOrders = orders.OrderByDescending(o => o.OrderDate)
                         .Take(5)
                         .Select(o => new OrderDto
                         {
                             OrderId = o.OrderId,
                             OrderDate = o.OrderDate,
                             TotalAmount = o.TotalAmount,
                             userName = o.User.UserName,
                             Status = o.OrderStatus
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

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderActionRequest updateStatus)
        {
            var result = await UpdateOrderStatus(updateStatus.OrderId, updateStatus.Status);

            if (result)
            {
                try
                {
                    var orders = await _orderRepo.GetAllOrders();
                    var pendingOrders = orders.Count(o => o.OrderStatus == "Pending");
                    var completedOrders = orders.Count(o => o.OrderStatus == "Completed");
                    var canceledOrders = orders.Count(o => o.OrderStatus == "Canceled");
                    return Json(new { success = true, pendingOrders, completedOrders, canceledOrders });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching all orders after updating status.");
                    return Json(new { success = false, message = "An error occurred while fetching all orders." });
                }
            }
            return Json(new { success = false });
        }

        private async Task<bool> UpdateOrderStatus(int orderId, string newStatus)
        {
            var order = await _orderRepo.GetOrderById(orderId);
            if (order == null)
            {
                return false;
            }
            order.OrderStatus = newStatus;
            await _orderRepo.UpdateOrder(order);
            return true;
        }
    }
}