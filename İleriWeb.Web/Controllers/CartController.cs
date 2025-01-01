using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Repository;
using IleriWeb.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
	
	public class CartController : BaseController
	{
		private readonly IProductService _productService;
		private readonly AppDbContext _context;
		private readonly IBasketService _basketService;
		private readonly IOrderService _orderService;
		private readonly IUnitOfWork unitOfWork;

		public CartController(AppDbContext context, IBasketService basketService, IOrderService orderService, IUnitOfWork unitOfWork, IProductService productService)
		{
			_context = context;
			_basketService = basketService;
			_orderService = orderService;
			this.unitOfWork = unitOfWork;
			_productService = productService;
		}

		public async Task<IActionResult> AddToCart(int id)
		{
			if (CurrentUser == null)
			{
				return RedirectToAction("login", "user");
			}

			var basket = await _basketService.GetBasketByUserId(CurrentUser.Id);
			var basketItem = new BasketItem
			{
				BasketId = basket.Id,
				ProductId = id,
				Quantity = 1,
				TotalPrice = _context.Product.Find(id).Price
			};
			_context.BasketItems.Add(basketItem);
			unitOfWork.Commit();
			return RedirectToAction("index", "home");
		}


		public async Task<IActionResult> AddToCartWithQuantity()
		{
			if (CurrentUser == null)
			{
				return RedirectToAction("login", "user");
			}
			var ProductId = Convert.ToInt32(TempData["ProductId"]);
			var Quantity = Convert.ToInt32(TempData["Quantity"]);
			var basket = await _basketService.GetBasketByUserId(CurrentUser.Id);
			var basketItem = new BasketItem
			{
				BasketId = basket.Id,
				ProductId = ProductId,
				Quantity = Quantity,
				TotalPrice = Quantity * (_context.Product.Find(ProductId).Price)
			};
			_context.BasketItems.Add(basketItem);
			unitOfWork.Commit();

			return RedirectToAction("Index", "Home");
		}


		public async Task<IActionResult> removeFromCart(int id)
		{

			var basketItem = await _context.BasketItems.FindAsync(id);
			_context.BasketItems.Remove(basketItem);
			unitOfWork.Commit();
			return RedirectToAction("index", "home");
		}

		public async Task<IActionResult> Index()
		{

			var basket = await _basketService.GetBasketDetailsWithIdAsync(CurrentUser.Id);
			ViewData["Basket"] = basket;
			return View();
		}

		public async Task<IActionResult> Order()
		{
			
			var basket = await _basketService.GetBasketDetailsWithIdAsync(CurrentUser.Id);
			if (basket == null || basket.Items == null || !basket.Items.Any())
			{
				// Sepet boşsa hata döndür veya kullanıcıyı bilgilendir
				return RedirectToAction("Index", "cart");
			}

			// Sepetteki her bir ürün için ayrı bir Order oluştur
			foreach (var item in basket.Items)
			{
				// Yeni bir Order oluştur
				var order = new Order
				{
					OrderDate = DateTime.UtcNow,
					IdentityUserId = basket.UserId, // Sepetin kullanıcısını Order'a ata
					OrderDetails = new OrderDetail // OrderDetail oluştur
					{
						ProductId = item.ProductId,
						Quantity = item.Quantity
					}
				};

				await _productService.UpdateStockAsync(item.ProductId, item.Quantity);
				// Order'ı veritabanına kaydet
				await _context.Order.AddAsync(order);
			}
			await unitOfWork.CommitAsync();

			await _basketService.ClearBasketAsync(basket.Id);


			return RedirectToAction("index","orders");
		}


	}
}
