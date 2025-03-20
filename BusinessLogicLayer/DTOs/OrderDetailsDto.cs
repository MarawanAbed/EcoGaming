

namespace BusinessLogicLayer.DTOs
{
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }

        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
