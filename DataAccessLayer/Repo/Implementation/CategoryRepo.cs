

using DataAccessLayer.Database;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repo.Implementation
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public  async Task AddCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {

            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }



        public async Task UpdateCategory(Category category)
        {
          var categoryEntity = await _context.Categories.FindAsync(category.CategoryId);
            if (categoryEntity != null)
            {
                categoryEntity.Name = category.Name;
                categoryEntity.ImageUrl = category.ImageUrl;
            }
            await _context.SaveChangesAsync();
        }
    }
}
