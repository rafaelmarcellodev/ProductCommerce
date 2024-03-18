using Microsoft.EntityFrameworkCore;
using ProductAPI.Data.Models;

namespace ProductAPI.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductContext _context;

        public ProductRepository(ProductContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetProductByName(string name)
        {
            var products = _context.Products.Where(p => p.Name.ToLower().Contains(name.ToLower()));

            return await products.ToListAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
