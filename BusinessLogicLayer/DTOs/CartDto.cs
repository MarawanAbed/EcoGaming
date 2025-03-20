

namespace BusinessLogicLayer.DTOs
{
    public class CartDto
    {
        public string UserId { get; set; }
        public List<CartDetailsDto> CartDetails { get; set; } = new List<CartDetailsDto>();
    }
}
