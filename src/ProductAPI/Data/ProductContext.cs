using Microsoft.EntityFrameworkCore;
using ProductAPI.Data.Models;

namespace ProductAPI.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } = null!;
    }
}
