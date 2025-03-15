

using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface ICategoryServices
    {

        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto> GetCategoryById(int id);
        Task AddCategory(CategoryDto category);
        Task UpdateCategory(CategoryDto category);
        Task <IEnumerable<GetAllProductsDto>> GetProductsByCategories(int id);
        Task DeleteCategory(int id);

    }
}
