using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests.Categories
{
    public class AddCategoriesActionRequest
    {
        [Required(ErrorMessage = "Category Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Image is required.")]
        public IFormFile Image { get; set; }

    }
}
