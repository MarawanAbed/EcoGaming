
namespace DataAccessLayer.Enitites
{
    public class CartDetails
    {
        public int CartDetailsId { get; set; } // Primary Key
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public Product Product { get; set; } // Navigation property
        public int CartId { get; set; }
        public Cart Cart { get; set; } // Navigation property
    }
}
