using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using 
    IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Repository;
using IleriWeb.Repository.Repositories;
using IleriWeb.Repository.UnitOfWorks;
using IleriWeb.Service.Mappers;
using IleriWeb.Service.Services;
using IleriWeb.Service.Validations;
using System.Reflection;
using IleriWeb.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using IleriWeb.Core.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using IleriWeb.Web.Middleware;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Opsiyonel: Identity ayarlarý
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();


// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();
builder.Services.AddAuthentication(
    CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x => { x.LoginPath = "/user/login"; });

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy =>
		policy.RequireRole("Admin"));
});

builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<ProductDtoValidator>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IProductRepýository, ProductRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Services.AddDbContext<AppDbContext>(
    x =>
    {
        x.UseNpgsql("UserID=postgres;Password=13579;Host=localhost;Port=5433;Database=IleriWebDb;");
    }
    );


var app = builder.Build();
app.UseExceptionHandler("/Home/Error");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.UseMiddleware<CurrentUserMiddleware>(); 

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
