using microservice.product.Application.DTO;
using microservice.product.Application.Interface;
using System.Net.Http.Json;
namespace microservice.product.Infrastructure.Service
{
    public class ProductCustomerService(IHttpClientFactory httpClient) : IProductCustomerService
    {
        public async Task<List<CustomerDTO>> GetProductCustomer()
        {
            var client = httpClient.CreateClient("Customer");
            var CustomerResponse = await client.GetFromJsonAsync<List<CustomerDTO>>("Customer");
            return CustomerResponse!;
        }
    }
}

//using microservice.product.Application.DTO;
//using microservice.product.Application.Interface;
//using microservice.product.domain.Model;

//public class ProductCustomerService : IProductCustomerService
//{
//    private readonly IHttpClientFactory _httpClientFactory;
//    private readonly IUnitOfWork _unitOfWork;

//    public ProductCustomerService(IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
//    {
//        _httpClientFactory = httpClientFactory;
//        _unitOfWork = unitOfWork;
//    }

//    // Get all products with customer information
//    public async Task<IActionResult> GetProductCustomer()
//    {
//        var client = _httpClientFactory.CreateClient("Customer");
//        var customerResponse = await client.GetFromJsonAsync<List<CustomerDTO>>("Customer");

//        if (customerResponse == null || !customerResponse.Any())
//        {
//            return new NotFoundObjectResult(new { Message = "No customer data found." });
//        }

//        var productResponse = await _unitOfWork.Product.GetAll();
//        if (productResponse == null || !productResponse.Any())
//        {
//            return new NotFoundObjectResult(new { Message = "No product data found." });
//        }

//        var combined = from product in productResponse
//                       join customer in customerResponse on product.CustomerId equals customer.CustomerId
//                       select new
//                       {
//                           product.ProductName,
//                           product.ProductPrice,
//                           customer.CustomerName,
//                           customer.Address,
//                           customer.Country
//                       };

//        return new OkObjectResult(combined);
//    }

//    // Create a new product and customer data
//    public async Task<IActionResult> AddProductAndCustomerAsync(ProductDTO productDto, CustomerDTO customerDto)
//    {
//        var client = _httpClientFactory.CreateClient("Customer");

//        // Create customer
//        var customerResponse = await client.PostAsJsonAsync("Customer", customerDto);
//        if (!customerResponse.IsSuccessStatusCode)
//        {
//            return new BadRequestObjectResult(new { Message = "Failed to add customer." });
//        }

//        // Create product
//        var product = new ProductModel
//        {
//            ProductName = productDto.ProductName,
//            ProductPrice = productDto.ProductPrice,
//            CustomerId = customerDto.CustomerId  // Ensure the CustomerId is set properly
//        };

//        await _unitOfWork.Product.Add(product);
//        await _unitOfWork.Save();

//        return new OkObjectResult(product);
//    }

//    // Update a product and customer data
//    public async Task<IActionResult> UpdateProductAndCustomerAsync(int productId, ProductDTO productDto, CustomerDTO customerDto)
//    {
//        var client = _httpClientFactory.CreateClient("Customer");

//        // Update customer
//        var customerResponse = await client.PutAsJsonAsync($"Customer/{customerDto.CustomerId}", customerDto);
//        if (!customerResponse.IsSuccessStatusCode)
//        {
//            return new BadRequestObjectResult(new { Message = "Failed to update customer." });
//        }

//        // Update product
//        var product = await _unitOfWork.Product.GetById(productId);
//        if (product == null)
//        {
//            return new NotFoundObjectResult(new { Message = "Product not found." });
//        }

//        product.ProductName = productDto.ProductName;
//        product.ProductPrice = productDto.ProductPrice;
//        product.CustomerId = customerDto.CustomerId;

//        _unitOfWork.Product.Update(product);
//        await _unitOfWork.Save();

//        return new OkObjectResult(product);
//    }

//    // Delete product and customer data
//    public async Task<IActionResult> DeleteProductAndCustomerAsync(int productId, int customerId)
//    {
//        var client = _httpClientFactory.CreateClient("Customer");

//        // Delete customer
//        var customerResponse = await client.DeleteAsync($"Customer/{customerId}");
//        if (!customerResponse.IsSuccessStatusCode)
//        {
//            return new BadRequestObjectResult(new { Message = "Failed to delete customer." });
//        }

//        // Delete product
//        var product = await _unitOfWork.Product.GetById(productId);
//        if (product == null)
//        {
//            return new NotFoundObjectResult(new { Message = "Product not found." });
//        }

//        await _unitOfWork.Product.Delete(productId);
//        await _unitOfWork.Save();

//        return new OkResult();
//    }
//}
