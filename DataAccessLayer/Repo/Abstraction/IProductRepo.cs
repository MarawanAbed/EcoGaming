using DataAccessLayer.Enitites;

namespace DataAccessLayer.Repo.Abstraction
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProducts(string userId, string role);
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product, string userId, string role);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> GetPopularProducts(int id);
        Task <IEnumerable<Product>> GetProdcutsByCategories(int id);

    }
}
