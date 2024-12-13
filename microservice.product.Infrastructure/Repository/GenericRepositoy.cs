using microservice.product.Application.Interface;
using microservice.product.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace microservice.product.Infrastructure.Repository
{
    public class GenericRepositoy<T>(ProductDbContext _dbContext) : IGenericRepository<T> where T : class
    {
        private readonly ProductDbContext dbContext = _dbContext;

        public async Task Add(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(int id)
        {
          var entity = await GetById(id);
          if(entity != null)
            {
                dbContext.Set<T>().Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbContext.Set<T>().ToListAsync();
            
        }

        public async Task<T> GetById(int id)
        {
            var data = await dbContext.Set<T>().FindAsync(id);
            return data!;
        }

        public void Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }         
    }
}
