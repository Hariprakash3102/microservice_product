

using microservice.product.Application.DTO;

namespace microservice.product.Application.Interface
{
    public interface IProductCustomerService
    {
        Task<List<CustomerDTO>> GetProductCustomer();
        
    }
}
