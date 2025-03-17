
namespace BusinessLogicLayer.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public string userName { get; set; }
    }

}
