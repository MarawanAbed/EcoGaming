using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Extend;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.orders;

namespace PresentationLayer.Controllers.Orders
{
    public class OrdersController : Controller
    {
        private readonly IOrderServices _orderServices;
        private readonly ILogger<OrdersController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrdersController(IOrderServices orderSerivces, ILogger<OrdersController> logger, UserManager<ApplicationUser> userManager)
        {
            _orderServices = orderSerivces;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderServices.GetAllOrders();
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
                             UserName = _userManager.Users.FirstOrDefault(u => u.Id == o.UserId).UserName,
                             OrderStatus = o.OrderStatus
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
                    var orders = await _orderServices.GetAllOrders();
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
            var order = await _orderServices.GetOrder(orderId);
            if (order == null)
            {
                return false;
            }
            order.OrderStatus = newStatus;
            await _orderServices.UpdateOrder(order);
            return true;
        }
    }
}