using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.ActionRequests.Products
{
    public class EditProductsActionRequest
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        public int Stock { get; set; }
        public IFormFile? Image { get; set; }
        public string? ExistingImagePath { get; set; }
        public int CategoryId { get; set; }
    }
}
