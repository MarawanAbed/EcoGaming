

using DataAccessLayer.Enitites;

namespace DataAccessLayer.Repo.Abstraction
{
    public interface ICartRepo
    {
        Task AddCart(Cart cart);

        Task AddCartDetails(CartDetails cartDetails);

        Task<Cart> GetCart(string userId);
        Task<IEnumerable<Cart>> GetCarts();
        Task<CartDetails> GetCartDetails(int cartId, int productId);

        Task<CartDetails> GetCartDetails(int cartDetailsId);

        Task RemoveCartDetails(CartDetails cartDetails);

        Task UpdateCartDetails(CartDetails cartDetails);

        Task UpdateCart();

        Task RemoveCart(Cart cart);

        Task RemoveCartItem(int cartId, int productId);
        Task ClearCart(int cartId);



    }
}
