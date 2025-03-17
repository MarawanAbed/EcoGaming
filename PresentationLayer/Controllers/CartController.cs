using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Extend;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.ActionRequests.Cart;


public class CartController : Controller
{
    private readonly ICartServices _services;
    private readonly UserManager<ApplicationUser> _userManager;

    public CartController(ICartServices services, UserManager<ApplicationUser> userManager)
    {
        _services = services;
        _userManager = userManager;
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
            cartItems = GetCartFromSession();
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
            List<CartDetailsDto> cart = GetCartFromSession();
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

            SaveCartToSession(cart);
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
            List<CartDetailsDto> cart = GetCartFromSession();
            var item = cart.FirstOrDefault(p => p.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
                SaveCartToSession(cart);
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
            SaveCartToSession(new List<CartDetailsDto>());
        }

        return RedirectToAction("Index");
    }

    // ✅ Helper Method: Get Cart from Session
    private List<CartDetailsDto> GetCartFromSession()
    {
        return HttpContext.Session.GetObject<List<CartDetailsDto>>("Cart") ?? new List<CartDetailsDto>();
    }

    // ✅ Helper Method: Save Cart to Session
    private void SaveCartToSession(List<CartDetailsDto> cart)
    {
        HttpContext.Session.SetObject("Cart", cart);
    }
}