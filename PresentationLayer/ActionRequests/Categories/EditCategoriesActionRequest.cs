using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests.Categories
{
    public class EditCategoriesActionRequest
    {
        public int CategoryId { get; set; }
        [Required (ErrorMessage = "Category Name is required.")]
        public string CategoryName { get; set; }

        public IFormFile Image { get; set; }
        public string ExistingImagePath { get; set; }


    }
}
