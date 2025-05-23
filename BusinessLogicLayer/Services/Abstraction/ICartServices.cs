﻿using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface ICartServices
    {

        Task AddCart(CartDto cart);

        Task<CartDto> GetCart(string userId);
        Task RemoveCartItem(string userId, int productId);
        Task ClearCart(string userId);
        Task MergeGuestCartWithUserCart(string userId, List<CartDetailsDto> guestCartDetails);
        Task UpdateCart(CartDto cart);
    }
}
