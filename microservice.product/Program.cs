using microservice.product.Application.Interface;
using microservice.product.Application.Mapper;
using microservice.product.Infrastructure.Data;
using microservice.product.Infrastructure.Repository;
using microservice.product.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

//builder.Services.AddScoped<IProductCustomerService, ProductCustomerService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddHttpClient("Customer" , options => options.BaseAddress = new Uri("https://localhost:7225/api/"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        RoleClaimType = "roles",
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

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

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
