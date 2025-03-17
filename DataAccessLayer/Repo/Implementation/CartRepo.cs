
using DataAccessLayer.Database;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repo.Implementation
{
    public class CartRepo : ICartRepo
    {

        private readonly ApplicationDbContext _context;


        public CartRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCart(Cart cart)
        {

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public Task AddCartDetails(CartDetails cartDetails)
        {
            throw new NotImplementedException();
        }

        public async Task ClearCart(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null)
            {
                cart.CartDetails.Clear();
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Cart> GetCart(string userId)
        {
            return await _context.Carts
                .Include(CartDetails => CartDetails.CartDetails)
                .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public Task<CartDetails> GetCartDetails(int cartId, int productId)
        {
            throw new NotImplementedException();
        }

        public Task<CartDetails> GetCartDetails(int cartDetailsId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Cart>> GetCarts()
        {
            return await _context.Carts.Include(CartDetails => CartDetails.CartDetails).ToListAsync();
        }

        public async Task RemoveCart(Cart cart)
        {

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
        }

        public Task RemoveCartDetails(CartDetails cartDetails)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveCartItem(int cartId, int productId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartDetails)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null) 
            {
                var item = cart.CartDetails.FirstOrDefault(cd => cd.ProductId == productId);
                if (item != null)
                {
                    cart.CartDetails.Remove(item);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateCart()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartDetails(CartDetails cartDetails)
        {
            await _context.SaveChangesAsync();
        }
    }
}
