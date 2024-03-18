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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Product 1",
                    Stock = 10,
                    Price = 12.50m
                },
                new Product
                {
                    Id = 2,
                    Name = "Product 2",
                    Stock = 20,
                    Price = 15.75m
                },
                new Product
                {
                    Id = 3,
                    Name = "Product 3",
                    Stock = 15,
                    Price = 10.00m
                },
                new Product
                {
                    Id = 4,
                    Name = "Product 4",
                    Stock = 8,
                    Price = 18.25m
                },
                new Product
                {
                    Id = 5,
                    Name = "Product 5",
                    Stock = 25,
                    Price = 9.99m
                }
                );
        }
    }
}
