using Azure;
using Mapster;
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
    public class ProductController(IUnitOfWork _unitOfWork/*, IMapper mapper, IProductCustomerService ProductCustomer*/) : Controller
    {

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProduct()
        {
            try
            {
                var response = await _unitOfWork.Product.GetAll();
                if (response == null)
                    return NotFound();

                else
                    return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public  async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var entity = await _unitOfWork.Product.GetById(id);
                if (entity == null)
                    return NotFound(new { Message = "User is not found." });

                else
                    return Ok(entity);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            
        }

        //[HttpGet("Get-Orders")]
        //public async Task<IActionResult> GetByOrder()
        //{
        //    return Ok(await unitOfWork.Product.GetByOrder());
        //}

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO entity)
        {
            //var product = mapper.Map <ProductModel>(entity);
            try
            {
                var product = entity.Adapt<ProductModel>();
                if (product == null)
                {
                    return NotFound(new { Message = "Products are null!!." });
                }
                await _unitOfWork.Product.Add(product);
                await _unitOfWork.Save();
                return Ok(product);
            }
            catch (Exception ex) { return BadRequest(ex.Message); }

            
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditProduct([FromBody] ProductModel entity)
        {
            try
            {
                if (entity == null)
                {
                    return NotFound(new { Message = "Product are null!! So can't edit." });
                }
                _unitOfWork.Product.Update(entity);
                await _unitOfWork.Save();
                return Ok(entity);
            }
            catch (Exception ex) { return BadRequest(ex.Message) ; }
           
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var response = await _unitOfWork.Product.GetById(id);
                if (response == null)
                {
                    return NotFound(new { Message = "Dlete the product is Failed by id not found" });
                }
                _unitOfWork.Product.Delete(response);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchProducts([FromQuery] string? search = "")
        {
            try
            {
                var products = await _unitOfWork.Product.GetAll();

                if(!products.Any() || products == null)
                {
                    return NotFound(new { message = " Search item is not Found "});
                }
                else
                {
                    if (!string.IsNullOrEmpty(search))
                    {
                        products = products.Where(product => product.ProductName!.Contains(search, StringComparison.OrdinalIgnoreCase)
                                                    || product.ProductCompany!.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
                    }

                    return Ok(products);
                }
            }
            catch (Exception ex) { return BadRequest(ex.Message); }
            

            
        }


    }
}
