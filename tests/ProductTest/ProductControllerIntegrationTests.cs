using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using ProductAPI.Data.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductTest
{
    public class ProductControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddProduct_Returns_CreatedAtAction()
        {
            var client = _factory.CreateClient();
            var productDto = new CreateProductDto { Name = "Test Product", Price = 10.99m };
            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/api/product", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetProducts_Returns_Ok()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/product?orderBy=price&ascending=true");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ReadProductDto>>(responseContent);

            Assert.NotNull(products);
            Assert.NotEmpty(products);
            var sortedProducts = products.OrderBy(p => p.Price).ToList();
            Assert.Equal(sortedProducts, products);
        }

        [Fact]
        public async Task GetProductById_Returns_Ok()
        {
            var client = _factory.CreateClient();
            var validProductId = 1;

            var response = await client.GetAsync($"/api/product/{validProductId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var product = JsonConvert.DeserializeObject<ReadProductDto>(responseContent);

            Assert.NotNull(product);
            Assert.Equal(validProductId, product.Id);
        }

        [Fact]
        public async Task GetProductByName_Returns_Ok()
        {
            var client = _factory.CreateClient();
            var validProductName = "Product 1";

            var response = await client.GetAsync($"/api/product/search/{validProductName}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var products = JsonConvert.DeserializeObject<List<ReadProductDto>>(responseContent);

            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public async Task UpdateProduct_Returns_NoContent()
        {
            var client = _factory.CreateClient();
            var validProductId = 2;
            var productDto = new CreateProductDto { Name = "Product Update", Price = 10.99m };
            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"/api/product/{validProductId}", content);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
