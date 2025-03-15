using DataAccessLayer.Enitites;

namespace DataAccessLayer.Repo.Abstraction
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task AddProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
        Task<IEnumerable<Product>> GetPopularProducts(int id);
        Task <IEnumerable<Product>> GetProdcutsByCategories(int id);

    }
}
