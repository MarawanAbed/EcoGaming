using BusinessLogicLayer.DTOs;

namespace PresentationLayer.ModelsVm.Categories
{
    public class GetAllCategoriesVm
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public static GetAllCategoriesVm FromCategory(CategoryDto category)
        {
            return new GetAllCategoriesVm
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                ImageUrl = category.ImageUrl
                
            };
        }
    }
}
