
namespace BusinessLogicLayer.DTOs
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }

        public string AddedByUserId { get; set; } // Store the user's ID
        public string AddedByRole { get; set; }   // Store the role (Admin or Buyer)
    }
}
