using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;
using NLayer.Service.Services;

namespace NLayer.Web.Controllers
{
    public class CategoriesController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        

        public CategoriesController(ICategoryService categoryService,IMapper mapper)
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
            if (ModelState.IsValid)
            {

                await _categoryService.AddAsync(_mapper.Map<Category>(categoryDto));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }
    }
}
