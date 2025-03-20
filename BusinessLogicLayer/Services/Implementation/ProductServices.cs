

using AutoMapper;
using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services.Abstraction;
using DataAccessLayer.Enitites;
using DataAccessLayer.Repo.Abstraction;
using MailKit.Search;

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

        public async Task<IEnumerable<GetAllProductsDto>> GetAllProducts(string userId, string role)
        {
            var products = await _productRepo.GetAllProducts(userId, role);
            return _mapper.Map<IEnumerable<GetAllProductsDto>>(products);
        }

        public async Task<IEnumerable<GetAllProductsDto>> GetPopularProducts(int id)
        {
            var popularProducts = await _productRepo.GetPopularProducts(id);
            return _mapper.Map<IEnumerable<GetAllProductsDto>>(popularProducts);
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepo.GetProductById(id);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<GetAllProductsDto>> GetProductsByCategory(int id)
        {
            var products = await _productRepo.GetProdcutsByCategories(id);
            return _mapper.Map<IEnumerable<GetAllProductsDto>>(products);
        }

        public async Task<IEnumerable<GetAllProductsDto>> SearchProductsByName(string name,int id)
        {
            var products=await _productRepo.GetProdcutsByCategories(id);
            return _mapper.Map<IEnumerable<GetAllProductsDto>>(products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));
        }

        public async Task UpdateProduct(ProductDto product, string userId, string role)
        {
            var productEntity = _mapper.Map<Product>(product);
            await _productRepo.UpdateProduct(productEntity,userId,role);
        }
    }
}
