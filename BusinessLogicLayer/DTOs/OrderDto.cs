
namespace BusinessLogicLayer.DTOs
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public string UserName { get; set; }
    }

}
