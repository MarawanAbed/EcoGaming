using DataAccessLayer.Database;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repo.Implementation
{
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;

        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {

            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProducts(string userId, string role)
        {
            if (role == "Admin")
            {
                return await _context.Products.Include(x => x.Category).ToListAsync();
            }
            else
            {
                return await _context.Products.Where(p => p.AddedByUserId == userId)
                    .Include(x => x.Category).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetPopularProducts(int id)
        {
            return await _context.Products.Where(p => p.CategoryId == id).Take(3).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProdcutsByCategories(int id)
        {
            return await _context.Products.Where(p => p.CategoryId == id).ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products
                .Include(p => p.Category)  // Ensure Category is included
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }



        public async Task UpdateProduct(Product product, string userId, string role)
        {
            
            var productEntity = await _context.Products.FindAsync(product.ProductId);
            if (productEntity != null) 
            {
                if (role == "Admin" || productEntity.AddedByUserId == userId)
                {
                    productEntity.Name = product.Name;
                    productEntity.Price = product.Price;
                    productEntity.CategoryId = product.CategoryId;
                    productEntity.Description = product.Description;
                    productEntity.Stock = product.Stock;
                    productEntity.Category = product.Category;

                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        productEntity.ImageUrl = product.ImageUrl;
                    }
                }
            }
            _context.Entry(entity: productEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
