using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Extend;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Cart;
using Stripe.Checkout;
using Stripe.Climate;

namespace PresentationLayer.Controllers.Orders
{
    [Authorize]

    public class OrderController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICartServices _cartServices;
        private readonly ICheckoutServices _checkoutServices;
        private readonly IOrderServices _orderServices;
        private readonly ILogger<OrderController> _logger;

        public OrderController(UserManager<ApplicationUser> userManager, ICartServices cartServices, IOrderServices order, ICheckoutServices checkout, ILogger<OrderController> logger)
        {
            _userManager = userManager;
            _cartServices = cartServices;
            _orderServices = order;
            _checkoutServices = checkout;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] List<AddCartActionRequest> cartItems)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _cartServices.GetCart(user.Id);

            if (cart == null || !cart.CartDetails.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Update cart quantities based on the received cartItems
            foreach (var item in cartItems)
            {
                var cartItem = cart.CartDetails.
                    FirstOrDefault(i => i.ProductId == item.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity = item.Quantity;
                }
            }

            // Create Order in Database
            var newOrder = new OrderDto
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartDetails.Sum(i => i.Price * i.Quantity),
                OrderStatus = "Pending",
                UserName = user.UserName,
                OrderDetails = cart.CartDetails.Select(i => new OrderDetailDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Name,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderServices.AddOrder(newOrder);

            try
            {
                var checkoutUrl = await _checkoutServices.CreateCheckoutSession(cart.CartDetails, user.Id);
                return Json(new { redirectUrl = checkoutUrl }); // Return the redirect URL for Stripe
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during checkout");
                return StatusCode(500, "An error occurred during checkout. Please try again later.");
            }
        }

        public async Task<IActionResult> Success(string session_id)
        {
            // Retrieve session from Stripe
            var service = new SessionService();
            var session = await service.GetAsync(session_id);

            if (session.PaymentStatus == "paid")
            {
                var user = await _userManager.GetUserAsync(User);

                // Update order status to "Paid"
                var order = await _orderServices.GetOrderById(user.Id);
                order.OrderStatus = "Paid";
                await _orderServices.UpdateOrder(order);

                // Clear user's cart
                await _cartServices.ClearCart(user.Id);
            }

            return View();
        }

        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderServices.GetOrdersByUserId(user.Id);

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductName = od.ProductName,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            }).ToList();

            return View(orderDtos);
        }


    }
}
