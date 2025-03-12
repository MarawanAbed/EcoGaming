using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface IProductServices
    {
        Task<IEnumerable<ProductDto>> GetAllProducts();
        Task<ProductDto> GetProductById(int id);
        Task AddProduct(ProductDto product);
        Task UpdateProduct(ProductDto product);
        Task DeleteProduct(int id);
    }
}
