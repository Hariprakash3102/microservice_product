//using AutoMapper;
using microservice.product.API.Controllers;
using microservice.product.Application.DTO;
using microservice.product.Application.Interface;
using microservice.product.domain.Model;
using Microsoft.AspNetCore.Mvc;
using Moq;
namespace microservice.product.Tests
{
    public class ProductControllerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _controller = new ProductController(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProductList()
        {
            // Arrange
            var products = new List<ProductModel>
            {
                new ProductModel {
                    ProductId = 1, 
                    ProductName = "Milk", 
                    ProductCompany = "Aavin", 
                    ProductPrice = 25.00m,
                    ProductQuantity = 1.0m,
                    ProductDiscount = 0,
                    CustomerId =1

                },
                new ProductModel {
                    ProductId = 2, 
                    ProductName = "Bread", 
                    ProductCompany = "Modern", 
                    ProductPrice = 15.00m,
                    ProductQuantity = 1.0m,
                    ProductDiscount = 0,
                    CustomerId =2
                }
            };
            _mockUnitOfWork.Setup(u => u.Product.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProduct();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count());
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var product = new ProductModel
            {
                ProductId = 1,
                ProductName = "Milk",
                ProductCompany = "Aavin",
                ProductPrice = 25.00m,
                ProductQuantity = 1.0m,
                ProductDiscount = 0,
                CustomerId = 1

            };
            _mockUnitOfWork.Setup(u => u.Product.GetById(1)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(1, returnedProduct.ProductId);
        }

        [Fact]
        public async Task GetProductById_ReturnsBadRequestResult_WhenProductDoesNotExist()
        {
            // Arrange
            _mockUnitOfWork.Setup(u => u.Product.GetById(It.IsAny<int>()))!.ReturnsAsync((ProductModel?)null);

            // Act
            var result = await _controller.GetProductById(1); // Passing an ID that doesn't exist

            // Assert
            Assert.IsType<NotFoundObjectResult>(result); // Expecting BadRequestResult
        }



        [Fact]
        public async Task AddProduct_ReturnsOkResult_WhenProductIsAdded()
        {
            // Arrange: Create a ProductDTO instance
            var productDto = new ProductDTO
            {
                ProductName = "Milk",
                ProductCompany = "Aavin",
                ProductPrice = 25.00m,
                ProductQuantity = 1.0m,
                ProductDiscount = 0,
                CustomerId = 1
            };

            // Create a ProductModel instance to be mapped from the ProductDTO
            var product = new ProductModel
            {
                ProductId = 1,
                ProductName = "Milk",
                ProductCompany = "Aavin",
                ProductPrice = 25.00m,
                ProductQuantity = 1.0m,
                ProductDiscount = 0,
                CustomerId = 1
            };

            _mockUnitOfWork.Setup(u => u.Product.Add(It.IsAny<ProductModel>())).Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddProduct(productDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode); 

            var returnedProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(product.ProductName, returnedProduct.ProductName);
        }


        [Fact]
        public async Task EditProduct_ReturnsOkResult_WhenProductIsUpdated()
        {
            // Arrange
            var product = new ProductModel 
            {
                ProductId = 1,
                ProductName = "Milk",
                ProductCompany = "Aavin",
                ProductPrice = 30.00m,
                ProductQuantity = 1.0m,
                ProductDiscount = 0,
                CustomerId = 1
            };
            _mockUnitOfWork.Setup(u => u.Product.Update(product));
            _mockUnitOfWork.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.EditProduct(product);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var updatedProduct = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(30.00m, updatedProduct.ProductPrice);
        }

        //[Fact]
        //public async Task DeleteProduct_ReturnsOkResult_WhenProductIsDeleted()
        //{
        //    var product = new ProductModel
        //    {
        //        ProductId = 1,
        //        ProductName = "Milk",
        //        ProductCompany = "Aavin",
        //        ProductPrice = 30.00m,
        //        ProductQuantity = 1.0m,
        //        ProductDiscount = 0,
        //        CustomerId = 1
        //    };
        //    // Arrange
        //    _mockUnitOfWork.Setup(u => u.Product.Delete(product));
        //    _mockUnitOfWork.Setup(u => u.Save()).Returns(Task.CompletedTask);

        //    // Act
        //    var result = await _controller.DeleteProduct(1);

        //    // Assert
        //    Assert.IsType<OkResult>(result);
        //}
        [Fact]
        public async Task DeleteProduct_ReturnsOkResult_WhenDeletionIsSuccessful()
        {
            // Arrange
            int productId = 1;

            var product = new ProductModel
            {
                ProductId = productId,
                ProductName = "Milk",
                ProductCompany = "Aavin",
                ProductPrice = 25.00m,
                ProductQuantity = 1.0m,
                ProductDiscount = 0,
                CustomerId = 1
            };

            // Mock the GetById method to return a product
            _mockUnitOfWork.Setup(u => u.Product.GetById(productId))
                           .ReturnsAsync(product);

            // Mock the Delete method (void method, no return)
            _mockUnitOfWork.Setup(u => u.Product.Delete(It.IsAny<ProductModel>()));

            // Mock the Save method
            _mockUnitOfWork.Setup(u => u.Save()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task SearchProducts_ReturnsFilteredProducts()
        {
            // Arrange
            var products = new List<ProductModel>
            {
                new ProductModel
                { 
                    ProductId = 1,
                    ProductName = "Milk",
                    ProductCompany = "Aavin",
                    ProductPrice = 25.00m,
                    ProductQuantity = 1.0m,
                    ProductDiscount = 0,
                    CustomerId = 1 
                },
                new ProductModel 
                { 
                    ProductId = 2,
                    ProductName = "Bread",
                    ProductCompany = "Modern",
                    ProductPrice = 15.00m,
                    ProductQuantity = 1.0m,
                    ProductDiscount = 0,
                    CustomerId =2 
                }
            };
            _mockUnitOfWork.Setup(u => u.Product.GetAll()).ReturnsAsync(products);

            // Act
            var result = await _controller.SearchProducts("Milk");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductModel>>(okResult.Value);
            Assert.Single(returnedProducts);
        }
    }
}