using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IleriWeb.Web.ViewComponents
{
	public class Currency:BaseComponent
	{
		private readonly ICurrencyService currencyService;

		public Currency(ICurrencyService currencyService)
		{
			this.currencyService = currencyService;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var rate=currencyService.Rate;
			
			return View(rate);
		}
	}
}
