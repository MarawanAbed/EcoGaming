using DataAccessLayer.Extend;
using System.ComponentModel.DataAnnotations;


namespace DataAccessLayer.Enitites
{
    public class Wishlist
    {
        public int WishlistId { get; set; } // Primary Key

        // Foreign Keys
        [Required(ErrorMessage = "User ID is required.")]

        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property

        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }
        public Product Product { get; set; } // Navigation property
    }
}
