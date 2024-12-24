using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IleriWeb.Web.ViewComponents
{
	public class UserButtons : BaseComponent
	{
		private readonly IBasketService basketService;

		public UserButtons(IBasketService basketService)
		{
			this.basketService = basketService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var CurrentUser = GetCurrentUser();
			ViewBag.CurrentUser = CurrentUser;
			var basket=await basketService.GetBasketDetailsWithIdAsync(CurrentUser.Id);
			ViewData["Basket"] = basket;
			return View();
		}
	}
}
