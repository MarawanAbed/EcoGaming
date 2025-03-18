using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Extend;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.ActionRequests.Cart;
using Stripe.Checkout;
using Stripe.Climate;

namespace PresentationLayer.Controllers
{
    public class OrderController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOrderRepo _orderRepo;
        private readonly ICartServices _cartServices;
        private readonly ICheckoutServices _checkoutServices;
        public OrderController(UserManager<ApplicationUser> userManager,ICartServices cartServices,IOrderRepo order,ICheckoutServices checkout)
        {
            _userManager = userManager;
            _cartServices = cartServices;
            _orderRepo = order;
            _checkoutServices = checkout;
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
                var cartItem = cart.CartDetails.FirstOrDefault(i => i.ProductId == item.ProductId);
                if (cartItem != null)
                {
                    cartItem.Quantity = item.Quantity;
                }
            }

            // Create Order in Database
            var newOrder = new DataAccessLayer.Enitites.Order
            {
                UserId = user.Id,
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartDetails.Sum(i => i.Price * i.Quantity),
                OrderStatus = "Pending",
                OrderDetails = cart.CartDetails.Select(i => new OrderDetail
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Price
                }).ToList()
            };

            await _orderRepo.AddOrder(newOrder);

            // Initiate Stripe Payment
            var checkoutService = new OrderService();
            var checkoutUrl = await _checkoutServices.CreateCheckoutSession(cart.CartDetails, user.Id);

            return Json(new { redirectUrl = checkoutUrl }); // Return the redirect URL for Stripe
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
                var order = await _orderRepo.GetOrderById(user.Id);
                order.OrderStatus = "Paid";
                await _orderRepo.UpdateOrder(order);

                // Clear user's cart
                await _cartServices.ClearCart(user.Id);
            }

            return View();
        }

        [Authorize]
        public async Task<IActionResult> OrderHistory()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = await _orderRepo.GetOrdersByUserId(user.Id);

            var orderDtos = orders.Select(o => new OrderDto
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                Status = o.OrderStatus,
                TotalAmount = o.TotalAmount,
                OrderDetails = o.OrderDetails.Select(od => new OrderDetailDto
                {
                    ProductName = od.Product.Name,
                    Quantity = od.Quantity,
                    Price = od.Price
                }).ToList()
            }).ToList();

            return View(orderDtos);
        }


    }
}
