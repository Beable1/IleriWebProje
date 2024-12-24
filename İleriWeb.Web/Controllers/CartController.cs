using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Repository;
using IleriWeb.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
	public class CartController : BaseController
	{
		private readonly AppDbContext _context;
		private readonly IBasketService _basketService;
		private readonly IUnitOfWork unitOfWork;

		public CartController(AppDbContext context, IBasketService basketService, IUnitOfWork unitOfWork)
		{
			_context = context;
			_basketService = basketService;
			this.unitOfWork = unitOfWork;
		}


		public async Task<IActionResult> addToCart(int id)
		{
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

		public async Task<IActionResult> removeFromCart(int id)
		{
			
			var basketItem = await _context.BasketItems.FindAsync(id);
			_context.BasketItems.Remove(basketItem);
			unitOfWork.Commit();
			return RedirectToAction("index", "home");
		}



	}
}
