using Microsoft.AspNetCore.Mvc;
using IleriWeb.Core.DTOs;
using System.Diagnostics;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Authorization;

using Messaging;
using Grpc.Net.Client;


namespace IleriWeb
	.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICategoryService _categoryService;
		private readonly ICurrencyService _currencyService;
        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService, ICurrencyService currencyService)
        {
            _logger = logger;
            _categoryService = categoryService;
            _currencyService = currencyService;
        }

        public async  Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllAsync();
            ViewBag.categories = categories.OrderByDescending(c => c.CreatedTime).Take(3);


            var rate =_currencyService.Rate;

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error(ErrorViewModel errorViewModel)
		{
			return View(errorViewModel);
		}
	}
}
