

using DataAccessLayer.Extend;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Enitites
{
    public class Cart
    {
        public int CartId { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key
        public ApplicationUser User { get; set; } // Navigation property
        public List<CartDetails> CartDetails { get; set; } // Navigation property
    }
}
