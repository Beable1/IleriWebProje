using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using IleriWeb.Core;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using IleriWeb.Core.Services;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using IleriWeb.Core.UnitOfWorks;

namespace IleriWeb.Web.Areas.Admin.Controllers
{
	[Area("admin")]
	[Authorize(Policy = "AdminPolicy")]
	public class ProductsController : Controller
	{

		private readonly IProductService _services;
		private readonly ICategoryService _categoryService;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderService _orderService;
		private readonly AppDbContext _context;

        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper, IOrderService orderService, AppDbContext context, IUnitOfWork unitOfWork)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
            _orderService = orderService;
            _context = context;
            _unitOfWork = unitOfWork;
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

			if (ModelState.IsValid)
			{

				var product = new Product
				{
					Name = productDto.Name,
					Stock = productDto.Stock,
					Price = productDto.Price,
					CategoryId = productDto.CategoryId,
					Description = productDto.Description,
					ImageData = imageData,
					ProductFeature = new ProductFeature
					{
						Color = productDto.ProductFeature.Color,
						Height = productDto.ProductFeature.Height,
						Width = productDto.ProductFeature.Width
					}
				};





				await _services.AddAsync(product);

				return RedirectToAction(nameof(Index));

			}




			var categories = await _categoryService.GetAllAsync();
			var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

			ViewBag.categories = new SelectList(categoriesDto, "Id", "Name");

			return View();
		}


		[ServiceFilter(typeof(NotFoundFilter<Product>))]
		public async Task<IActionResult> Update(int id)
		{
			var product = await _services.GetProductDetailsWithIdAsync(id);

			var categories = await _categoryService.GetAllAsync();
			var categoriesDto = _mapper.Map<List<CategoryDto>>(categories.ToList());

			ViewBag.categories = new SelectList(categoriesDto, "Id", "Name", product.CategoryId);

			return View(_mapper.Map<ProductUpdateDto>(product));
		}
		[HttpPost]
		public async Task<IActionResult> Update(ProductUpdateDto productDto)
		{
            byte[] imageData;
            using (var memoryStream = new MemoryStream())
            {
                await productDto.imageFile.CopyToAsync(memoryStream);
                imageData = memoryStream.ToArray();

            }

            if (ModelState.IsValid)
			{
				var product = _mapper.Map<Product>(productDto);
				product.ImageData = imageData;
				_context.Product.Update(product);
				_unitOfWork.Commit();
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
	}
}
