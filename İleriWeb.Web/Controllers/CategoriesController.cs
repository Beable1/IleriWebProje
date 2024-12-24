using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IleriWeb.Core;
using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using IleriWeb.Service.Services;
using IleriWeb.Core.DTOs;

namespace IleriWeb.Web.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoriesController(ICategoryService categoryService, IMapper mapper)
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


        public async Task<IActionResult> Products(int id)
		{
			var category = await _categoryService.GetSingleCategoryByIdWithProductsAsync(id);
			
			return View(category);
		}


	}
}
