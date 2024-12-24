using AutoMapper;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using IleriWeb.Core;
using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Repository;

namespace IleriWeb.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;


        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper, IOrderService orderService, AppDbContext context)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
            _orderService = orderService;
            
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            return View(await _services.GetProductsWithCategory());
        }

        public async Task<IActionResult> Save()
        {
            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await productDto.imageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();
                
            }


            
                var product = _mapper.Map<Product>(productDto);
                product.ImageData = imageData;
                await _services.AddAsync(product);
                return RedirectToAction(nameof(Index));
            

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

            return View();
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _services.GetByIdAsync(id);

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

            return View(_mapper.Map<ProductDto>(product));
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductDto productDto)
        {

            if (ModelState.IsValid)
            {

                await _services.UpdateAsync(_mapper.Map<Product>(productDto));
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryService.GetAllAsync();
            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

            ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", productDto.CategoryId);

            return View(productDto);
        }


        public async Task<IActionResult> Remove(int id)
        {
            var product = await _services.GetByIdAsync(id);
            await _services.RemoveAsync(product);
            return RedirectToAction(nameof(Index));
        }



		[HttpGet]
		public async Task<IActionResult> ProductDetails(int id)
        {

            var product = await _services.GetProductDetailsWithIdAsync(id);


			return View(product);
		}


		public async Task<IActionResult> deneme()
		{




			var orderr = new Order
			{
				OrderDate = DateTime.UtcNow,
				IdentityUserId = 7,
				OrderDetails = new OrderDetail
				{

					ProductId = 11,
					Quantity = 3
				}
			};

			await _orderService.AddAsync(orderr);


			return RedirectToAction("index");



		}

		




	}
}
