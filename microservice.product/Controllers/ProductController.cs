using AutoMapper;
using Azure;
using microservice.product.Application.DTO;
using microservice.product.Application.Interface;
using microservice.product.domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace microservice.product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IUnitOfWork _unitOfWork, IMapper mapper, IProductCustomerService ProductCustomer) : Controller
    {
        private readonly IUnitOfWork unitOfWork = _unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            var response = await unitOfWork.Product.GetAll();
            if (response == null)
                return BadRequest();

            else
                return Ok(response);

        }

        [HttpGet("{id}")]
        public  async Task<IActionResult> GetProductById(int id)
        {
            var entity = await unitOfWork.Product.GetById(id);
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
            await unitOfWork.Product.Add(data);
            await unitOfWork.Save();
            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct([FromBody] ProductModel entity)
        {
             unitOfWork.Product.Update(entity);
            await unitOfWork.Save();
            return Ok(entity);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
           await unitOfWork.Product.Delete(id);
           await unitOfWork.Save();
           return Ok();
        }

        [HttpGet("withCustomer")]
        public async Task<IActionResult> GetByCustomer()
        {
            var response = await ProductCustomer.GetProductCustomer();
            if (response == null)
            {
                return NotFound(new { Message = "No customer data found." });
            }
            var ProductResponse = await unitOfWork.Product.GetAll();
            if (ProductResponse == null || !ProductResponse.Any())
            {
                return NotFound(new { Message = "No Product data found" });
            }

            var combined = from product in ProductResponse
                           join customer in response on product.CustomerId equals customer.CustomerId
                           select new
                           {
                               product.ProductName,
                               product.ProductPrice,
                               customer.CustomerName,
                               customer.Address,
                               customer.Country
                           };
            return Ok(combined); 
        }
    }
}
