using DataAccessLayer.Extend;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Enitites
{
    public class Order
    {

        public int OrderId { get; set; }

        [Required(ErrorMessage = "Order Date is required.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Total Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total Amount must be greater than 0.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters.")]
        public string OrderStatus { get; set; }

        // Foreign Key
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } // Navigation property

        // Navigation property
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
