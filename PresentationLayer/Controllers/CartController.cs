using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using BusinessLogicLayer.Services.Implementation;
using DataAccessLayer.Enitites;
using DataAccessLayer.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Cart;


public class CartController : Controller
{
    private readonly ICartServices _services;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public CartController(ICartServices services, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
    {
        _services = services;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
    }

    // ✅ View Cart
    public async Task<IActionResult> Index()
    {
        List<CartDetailsDto> cartItems = new List<CartDetailsDto>();

        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            var cart = await _services.GetCart(user.Id);
            cartItems = cart?.CartDetails ?? new List<CartDetailsDto>();
        }
        else
        {
            var session = _httpContextAccessor.HttpContext.Session;
            cartItems = session.GetGuestCart();
        }

        return View(cartItems);
    }

    // ✅ Add to Cart
    [HttpPost]
    public async Task<IActionResult> AddToCart(AddCartActionRequest addCartAction)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);

            await _services.AddCart(
                new CartDto
                {
                    UserId = user.Id,
                    CartDetails = new List<CartDetailsDto>
                    {
                        new CartDetailsDto
                        {
                            ProductId = addCartAction.ProductId,
                            Name = addCartAction.ProductName,
                            Price = addCartAction.Price,
                            ImageUrl = addCartAction.ImageUrl,
                            Quantity = 1,
                            Stock = addCartAction.Stock,
                            Description = addCartAction.ProductDescription,
                        }
                    }
                }
                );
        }
        else
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetGuestCart();

            var existingItem = cart.FirstOrDefault(p => p.ProductId == addCartAction.ProductId);

            if (existingItem != null)
                existingItem.Quantity++;
            else
                cart.Add(
                    new CartDetailsDto
                    {
                        ProductId = addCartAction.ProductId,
                        Name = addCartAction.ProductName,
                        Price = addCartAction.Price,
                        ImageUrl = addCartAction.ImageUrl,
                        Quantity = 1,
                        Stock = addCartAction.Stock,
                        Description = addCartAction.ProductDescription,
                    });

            session.SaveGuestCart(cart);
        }

        return RedirectToAction("Index");
    }

    // ✅ Remove from Cart
    public async Task<IActionResult> RemoveCartItem(int productId)
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            await _services.RemoveCartItem(user.Id, productId);
        }
        else
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var cart = session.GetGuestCart() ?? new List<CartDetailsDto>(); // Ensure cart is never null
            var item = cart.FirstOrDefault(p => p.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                session.SaveGuestCart(cart);
            }
        }
        return RedirectToAction("Index");
    }

    // ✅ Clear Cart
    public async Task<IActionResult> ClearCart()
    {
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(User);
            await _services.ClearCart(user.Id);
        }
        else
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.ClearGuestCart();
        }

        return RedirectToAction("Index");
    }
}