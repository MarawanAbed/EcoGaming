
using DataAccessLayer.Enitites;

namespace DataAccessLayer.Repo.Abstraction
{
    public interface ICategoryRepo
    {
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> GetCategoryById(int id);
        Task AddCategory(Category category);
        Task UpdateCategory(Category category);
        Task DeleteCategory(int id);
    }
}
