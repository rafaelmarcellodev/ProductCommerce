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
    }
}
