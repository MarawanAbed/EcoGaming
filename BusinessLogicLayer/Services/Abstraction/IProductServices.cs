﻿using BusinessLogicLayer.DTOs;

namespace BusinessLogicLayer.Services.Abstraction
{
    public interface IProductServices
    {
        Task<IEnumerable<GetAllProductsDto>> GetAllProducts(string userId, string role);
        Task<ProductDto> GetProductById(int id);
        Task AddProduct(ProductDto product);
        Task UpdateProduct(ProductDto product, string userId, string role);
        Task DeleteProduct(int id);
        Task<IEnumerable<GetAllProductsDto>> GetPopularProducts(int id);
        Task<IEnumerable<GetAllProductsDto>> SearchProductsByName(string name,int id);

        Task<IEnumerable<GetAllProductsDto>> GetProductsByCategory(int id);

    }
}
