
using System.ComponentModel.DataAnnotations;


namespace DataAccessLayer.Enitites
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; } // Primary Key
        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public Order Order { get; set; } // Navigation property

        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
    }
}
