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

        public async Task<IEnumerable<Product>> GetAllProducts()
        {

            return await _context.Products.Include(x => x.Category).ToListAsync();
            //cuz i want to include the category of the product in the response
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateProduct(Product product)
        {
            
            var productEntity = await _context.Products.FindAsync(product.ProductId);
            if (productEntity != null) 
            {
                productEntity.Name = product.Name;
                productEntity.Price = product.Price;
                productEntity.CategoryId = product.CategoryId;
                productEntity.Description = product.Description;
                productEntity.ImageUrl = product.ImageUrl;
                productEntity.Stock = product.Stock;
                productEntity.Category = product.Category;
            }
            await _context.SaveChangesAsync();
        }
    }
}
