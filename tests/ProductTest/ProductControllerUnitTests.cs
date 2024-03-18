using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductAPI.Controllers;
using ProductAPI.Data.Dtos.Product;
using ProductAPI.Data.Models;
using ProductAPI.Data.Repository;
using ProductAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductTest
{
    [Trait("Category", "Unit")]
    public class ProductControllerUnitTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository = new();
        private readonly Mock<IMapper> _mockMapper = new();
        private readonly Mock<ExceptionHandlingService> _mockExceptionHandlingService = new();

        [Fact]
        public async Task AddProduct_Returns_CreatedAtActionResult()
        {
            var productDto = new CreateProductDto { Name = "Test Product", Price = 10.99m };
            var product = new Product { Id = 1, Name = "Test Product", Price = 10.99m };

            _mockProductRepository.Setup(repo => repo.AddProduct(It.IsAny<Product>())).ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
            
            var controller = new ProductController(_mockProductRepository.Object, _mockMapper.Object, _mockExceptionHandlingService.Object);

            var result = await controller.AddProduct(productDto);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(ProductController.GetProductById), createdAtActionResult.ActionName);
            Assert.Equal(product, createdAtActionResult.Value);
        }

        [Fact]
        public async Task AddProduct_Returns_BadRequest()
        {
            var productDto = new CreateProductDto { Name = "Test Product", Price = -10.99m };
            var product = new Product { Id = 1, Name = "Test Product", Price = -10.99m };

            _mockProductRepository.Setup(repo => repo.AddProduct(It.IsAny<Product>())).ReturnsAsync(product);
            _mockMapper.Setup(mapper => mapper.Map<Product>(productDto)).Returns(product);
            
            var controller = new ProductController(_mockProductRepository.Object, _mockMapper.Object, _mockExceptionHandlingService.Object);
            
            var result = await controller.AddProduct(productDto);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
        }

        [Fact]
        public async Task AddProduct_Returns_StatusCode500()
        {
            var productDto = new CreateProductDto { Name = "Test Product", Price = 10.99m };
            _mockMapper.Setup(mapper => mapper.Map<Product>(productDto)).Throws(new Exception("Test exception"));
            _mockExceptionHandlingService.Setup(s => s.HandleException(It.IsAny<Exception>())).Returns("Handled exception message");
            
            var controller = new ProductController(_mockProductRepository.Object, _mockMapper.Object, _mockExceptionHandlingService.Object);

            var result = await controller.AddProduct(productDto);

            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(500, objectResult.StatusCode);
        }
    }
}
