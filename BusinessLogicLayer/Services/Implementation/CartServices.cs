using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;

namespace BusinessLogicLayer.Services.Implementation
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepo _cartRepo;
        private readonly IMapper _mapper;


        public CartServices(ICartRepo cartRepo, IMapper mapper)
        {
            _cartRepo = cartRepo;
            _mapper = mapper;
        }

        public async Task AddCart(CartDto cartDto)
        {
            var cart = await _cartRepo.GetCart(cartDto.UserId);

            if (cart == null)
            {
                var newCart = _mapper.Map<Cart>(cartDto);
                await _cartRepo.AddCart(newCart);
            }
            else
            {
                foreach (var item in cartDto.CartDetails)
                {
                    var existingItem = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == item.ProductId);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += item.Quantity;
                    }
                    else
                    {
                        cart.CartDetails.Add(new CartDetails
                        {
                            ProductId = item.ProductId,
                            Price = item.Price,
                            Name = item.Name,
                            Quantity = item.Quantity,
                            ImageUrl = item.ImageUrl,
                            Description = item.Description,
                            Stock = item.Stock
                        });
                    }
                }
            }

            await _cartRepo.UpdateCart();
        }

        public async Task ClearCart(string userId)
        {
            var cart = await _cartRepo.GetCart(userId);
            if (cart != null)
            {
                cart.CartDetails.Clear();
                await _cartRepo.UpdateCart();
            }
        }

        public async Task<CartDto> GetCart(string userId)
        {
            var cart = await _cartRepo.GetCart(userId);
            return _mapper.Map<CartDto>(cart);
        }

        public async Task MergeGuestCartWithUserCart(string userId, List<CartDetailsDto> guestCartDetails)
        {
            var userCart = await _cartRepo.GetCart(userId); // Retrieve user cart

            if (guestCartDetails.Any())
            {
                if (userCart == null)
                {
                    var newCart = _mapper.Map<Cart>
                        (new CartDto { UserId = userId, CartDetails = guestCartDetails });
                    await _cartRepo.AddCart(_mapper.Map<Cart>(newCart));
                }
                else
                {
                    // Merge the guest cart into the existing user cart
                    foreach (var guestItem in guestCartDetails)
                    {
                        var existingItem = userCart.CartDetails.FirstOrDefault(i => i.ProductId == guestItem.ProductId);

                        if (existingItem != null)
                        {
                            existingItem.Quantity += guestItem.Quantity; // Merge quantity
                        }
                        else
                        {
                            userCart.CartDetails.Add(new CartDetails
                            {
                                ProductId = guestItem.ProductId,
                                Price = guestItem.Price,
                                Name = guestItem.Name,
                                Quantity = guestItem.Quantity,
                                ImageUrl = guestItem.ImageUrl,
                                Description = guestItem.Description,
                                Stock = guestItem.Stock
                            });
                        }
                    }

                    await _cartRepo.UpdateCart();
                }
            }
        }


        public async Task RemoveCartItem(string userId, int productId)
        {
            var cart = await _cartRepo.GetCart(userId);
            if (cart != null) 
            {
                var item = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);
                if (item != null)
                {
                    cart.CartDetails.Remove(item);
                    await _cartRepo.UpdateCart();
                }
            }
        }

        public async Task UpdateCart(CartDto cart)
        {
            var existingCart = await _cartRepo.GetCart(cart.UserId);
            if (existingCart != null)
            {
                existingCart.CartDetails = _mapper.Map<List<CartDetails>>(cart.CartDetails);
                await _cartRepo.UpdateCart();
            }
        }
    }
}
