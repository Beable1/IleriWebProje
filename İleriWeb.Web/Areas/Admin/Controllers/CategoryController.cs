using AutoMapper;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Policy = "AdminPolicy")]
    public class CategoryController : Controller
	{
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;

		public CategoryController(ICategoryService categoryService, IMapper mapper)
		{
			_categoryService = categoryService;
			_mapper = mapper;
		}

		public async Task<IActionResult> Index()
		{
			var categories = await _categoryService.GetAllAsync();
			var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

			return View(categoriesDto);
		}

        public IActionResult Create()
        {


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDto categoryDto)
        {

            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await categoryDto.imageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();

            }

            if (ModelState.IsValid)
            {
                var category = _mapper.Map<Category>(categoryDto);
                category.ImageData = imageData;
                await _categoryService.AddAsync(_mapper.Map<Category>(category));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
