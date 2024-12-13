using microservice.product.domain.Model;
using Microsoft.EntityFrameworkCore;

namespace microservice.product.Infrastructure.Data
{
    public class ProductDbContext(DbContextOptions<ProductDbContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Product { get; set; }
        public DbSet<OrderModel> Orders { get; set; }

    }
}
