

using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface ICheckoutServices
    {
        Task<string> CreateCheckoutSession(List<CartDetailsDto> cartDetailsDto,string id);
    }
}
