

using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using Stripe.Checkout;

namespace BusinessLogicLayer.Services.Implementation
{
    public class CheckoutServices : ICheckoutServices
    {
        public async Task<string> CreateCheckoutSession(List<CartDetailsDto> cartDetailsDto, string id)
        {
            var domain = "http://localhost:5121";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = cartDetailsDto.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        UnitAmount = (long)(item.Price * 100), // Convert to cents
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                            Images = new List<string> { domain + "/Images/" + item.ImageUrl }
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = $"{domain}/Order/Success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/Cart"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session.Url; // Redirect user to this URL
        }
    }
}
