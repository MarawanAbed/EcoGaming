

using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;

namespace BusinessLogicLayer.Services.Implementation
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;

        public ProductServices(IProductRepo productRepo, IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }

        public async Task AddProduct(ProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            await _productRepo.AddProduct(productEntity);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepo.DeleteProduct(id);
        }

        public async Task<IEnumerable<GetAllProductsDto>> GetAllProducts()
        {
            var products = await _productRepo.GetAllProducts();
            return _mapper.Map<IEnumerable<GetAllProductsDto>>(products);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepo.GetProductById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task UpdateProduct(ProductDto product)
        {
            var productEntity = _mapper.Map<Product>(product);
            await _productRepo.UpdateProduct(productEntity);
        }
    }
}
