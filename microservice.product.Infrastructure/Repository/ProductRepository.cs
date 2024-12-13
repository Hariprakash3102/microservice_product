using microservice.product.Application.Interface;
using microservice.product.domain.Model;
using microservice.product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace microservice.product.Infrastructure.Repository
{
    public class ProductRepository(ProductDbContext dbcontext) : GenericRepositoy<ProductModel>(dbcontext), IProductRepository
    {
        //public async Task<List<ProductModel>> GetByOrder()
        //{
        //    return await dbcontext.Product.Include(c => c.Orders).ToListAsync();
        //}
    }

}
