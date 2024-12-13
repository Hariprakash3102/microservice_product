using microservice.product.Application.Interface;
using microservice.product.Infrastructure.Data;
using microservice.product.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace microservice.product.Infrastructure.UnitOfWork
{
    public class UnitOfWork(ProductDbContext dbContext) : IUnitOfWork
    {
        private readonly ProductDbContext dbContext = dbContext;
        public IProductRepository Product { get; private set; } = new ProductRepository(dbContext);

        public void Dispose()
        {
           dbContext.Dispose();
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
