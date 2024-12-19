using AutoMapper;
using Azure;
using microservice.product.Application.DTO;
using microservice.product.Application.Interface;
using microservice.product.domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

namespace microservice.product.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IUnitOfWork _unitOfWork, IMapper mapper/*, IProductCustomerService ProductCustomer*/) : Controller
    {

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var response = await _unitOfWork.Product.GetAll();
            if (response == null)
                return BadRequest();

            else
                return Ok(response);

        }

        [HttpGet("{id}")]
        public  async Task<IActionResult> GetProductById(int id)
        {
            var entity = await _unitOfWork.Product.GetById(id);
            if (entity == null)
                return BadRequest();

            else
                return Ok(entity);
        }

        //[HttpGet("Get-Orders")]
        //public async Task<IActionResult> GetByOrder()
        //{
        //    return Ok(await unitOfWork.Product.GetByOrder());
        //}

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO entity)
        {
            var data = mapper.Map <ProductModel>(entity);
            await _unitOfWork.Product.Add(data);
            await _unitOfWork.Save();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] ProductModel entity)
        {
            _unitOfWork.Product.Update(entity);
            await _unitOfWork.Save();
            return Ok(entity);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
           await _unitOfWork.Product.Delete(id);
           await _unitOfWork.Save();
           return Ok();
        }

        //[HttpGet("GetByCustomer")]
        //public async Task<IActionResult> GetByCustomer()
        //{
        //    var response = await ProductCustomer.GetProductCustomer();
        //    if (response == null)
        //    {
        //        return NotFound(new { Message = "No customer data found." });
        //    }
        //    var ProductResponse = await _unitOfWork.Product.GetAll();
        //    if (ProductResponse == null || !ProductResponse.Any())
        //    {
        //        return NotFound(new { Message = "No Product data found" });
        //    }

        //    var combined = from product in ProductResponse
        //                   join customer in response on product.CustomerId equals customer.CustomerId
        //                   select new
        //                   {
        //                       product.ProductName,
        //                       product.ProductPrice,
        //                       customer.CustomerName,
        //                       customer.Address,
        //                       customer.Country
        //                   };
        //    return Ok(combined); 
        //}

        //[HttpGet("GetProductByCustomerId/{id}")]
        //public async Task<IActionResult> GetProductByCustomerId(int id)
        //{
        //    var CustomerData = await ProductCustomer.GetProductCustomerById(id);
        //    var ProductData = await _unitOfWork.Product.GetById(id);

        //    var combined = new
        //                   {
        //                        CustomerData.CustomerName,
        //                        CustomerData.Address,
        //                        CustomerData.Country,
        //                        ProductData.ProductName,
        //                        ProductData.ProductPrice,

        //                   };
        //    return Ok(combined);
        //}

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string? search = "")
        {
            var products = await _unitOfWork.Product.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(product => product.ProductName.Contains(search, StringComparison.OrdinalIgnoreCase)
                                            || product.ProductCompany.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return Ok(products);
        }


    }
}
