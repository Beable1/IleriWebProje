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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Web.Mvc;
using Microsoft.Extensions.Options;
using ExchangeRateService;
using IleriWeb.Web.Hubs;
using IleriWeb.Web.Services;
using Grpc.Net.Client;
using Grpc.AspNetCore.Web;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    // Opsiyonel: Identity ayarları
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
})
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();


builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", builder =>
	{
		builder.AllowAnyOrigin()
			   .AllowAnyMethod()
			   .AllowAnyHeader().WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
	});
});

builder.Services.AddSignalR();  // SignalR servisini ekleyin
builder.Services.AddGrpc();     // gRPC servisini ekleyin

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
    {
        policy.RequireRole("admin");
    });

	options.FallbackPolicy = new AuthorizationPolicyBuilder()
		.RequireAuthenticatedUser()
		.Build();

});

builder.Services.ConfigureApplicationCookie(options =>
{
	options.LoginPath = "/user/login"; // Giriş yapılması gereken sayfa
	options.AccessDeniedPath = "/User/AccessDenied"; // Erişim engellendiğinde yönlenecek sayfa
});



builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddScoped<IProductRepıository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICurrencyService, CurrencyService>();

builder.Services.AddTransient<GrpcChatService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddHttpClient();

builder.Services.AddDbContext<AppDbContext>(
    x =>
    {
        x.UseNpgsql("UserID=postgres;Password=13579;Host=localhost;Port=5433;Database=IleriWebDb;");
    }
    );

builder.Services.AddGrpc();

var app = builder.Build();

app.MapHub<ChatHub>("/chatHub");

app.UseExceptionHandler("/Home/Error");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseGrpcWeb();
app.MapGrpcService<Messaging.Messaging.MessagingClient>().EnableGrpcWeb().RequireCors("AllowAll");
app.UseStaticFiles();

app.UseRouting();
app.UseCors();
app.UseAuthentication();

app.UseAuthorization();
app.UseSession();
app.UseMiddleware<CurrentUserMiddleware>();
app.UseMiddleware<CurrencyMiddleware>();

app.MapControllerRoute(name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapHub<ChatHub>("/messagingHub");
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
