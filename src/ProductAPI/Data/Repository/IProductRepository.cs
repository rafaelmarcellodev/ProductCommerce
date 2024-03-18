using ProductAPI.Data.Models;

namespace ProductAPI.Data.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProduct(Product product);
        Task<List<Product>> GetProducts();
        Task<Product?> GetProductById(int id);
        Task<List<Product>> GetProductByName(string name);
        Task<Product> UpdateProduct(Product product);
        Task<bool> DeleteProduct(int id);
    }
}
