namespace PresentationLayer.ActionRequests.Cart
{
    public class AddCartActionRequest
    {
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }
        public int Quantity { get; set; } // Add the Quantity property


    }
}
