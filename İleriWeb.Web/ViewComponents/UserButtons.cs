using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace IleriWeb.Web.ViewComponents
{
	public class UserButtons : BaseComponent
	{
		private readonly IBasketService basketService;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserButtons(IBasketService basketService, UserManager<ApplicationUser> userManager)
		{
			this.basketService = basketService;
			_userManager = userManager;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var CurrentUser = GetCurrentUser();
			ViewBag.CurrentUser = CurrentUser;
			
			
			if (CurrentUser != null)
			{

				var IsInRole = await _userManager.IsInRoleAsync(CurrentUser, "Admin");
				if (IsInRole == true)
				{
					ViewBag.IsInRole = IsInRole;
				}

				var basket = await basketService.GetBasketDetailsWithIdAsync(CurrentUser.Id);
				ViewData["Basket"] = basket;
			}
			
			return View();
		}
	}
}
