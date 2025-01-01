using AutoMapper;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IleriWeb.Core;
using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Authorization;

namespace IleriWeb.Web.Controllers
{
	[AllowAnonymous]
	public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;


        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper, IOrderService orderService, AppDbContext context)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
            _orderService = orderService;
            
            _context = context;
        }





		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var products= await _services.GetProductsWithDetails();
			return View(products);
		}


		[HttpGet]
		public async Task<IActionResult> ProductDetails(int id)
        {

            var product = await _services.GetProductDetailsWithIdAsync(id);


			return View(product);
		}

		[HttpPost]
		public ActionResult SubmitData(AddToCartWithQuantityDTO model)
		{
			if (ModelState.IsValid)
			{
				TempData["ProductId"] = model.ProductId;
				TempData["Quantity"] = model.Quantity;

				// İkinci View'e yönlendir
				return RedirectToAction("AddToCartWithQuantity","Cart");
			}

			return View("ProductDetails", model);
		}


		public async Task<IActionResult> deneme()
		{




			var orderr = new Order
			{
				OrderDate = DateTime.UtcNow,
				IdentityUserId = 7,
				OrderDetails = new OrderDetail
				{

					ProductId = 11,
					Quantity = 3
				}
			};

			await _orderService.AddAsync(orderr);


			return RedirectToAction("index");



		}

		




	}
}
