using Microsoft.AspNetCore.Mvc;
using IleriWeb.Core.DTOs;
using System.Diagnostics;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace IleriWeb
	.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async  Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllAsync();
			ViewBag.categories = categories.Take(3);
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
