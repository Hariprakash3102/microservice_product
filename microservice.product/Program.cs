using microservice.product.Application.Interface;
using microservice.product.Application.Mapper;
using microservice.product.Infrastructure.Data;
using microservice.product.Infrastructure.Repository;
using microservice.product.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region DbContext
builder.Services.AddDbContext<ProductDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region CORS
builder.Services.AddCors(options => options.AddPolicy("Product_Policy", x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
#endregion

builder.Services.AddAutoMapper(typeof(ProductMapper));

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepositoy<>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Product_Policy");

app.UseAuthorization();

app.MapControllers();

app.Run();
