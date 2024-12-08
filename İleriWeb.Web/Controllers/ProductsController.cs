using AutoMapper;
using İleriWeb.Core.DTOs;
using İleriWeb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NLayer.Core;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Repository;

namespace NLayer.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _services;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IService<Customer> _customerService;
        private readonly IOrderService _orderService;
        private readonly AppDbContext _context;


        public ProductsController(IProductService services, ICategoryService categoryService, IMapper mapper, IOrderService orderService, IService<Customer> customerService, AppDbContext context)
        {
            _services = services;
            _categoryService = categoryService;
            _mapper = mapper;
            _orderService = orderService;
            _customerService = customerService;
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

            if (ModelState.IsValid)
            {

                await _services.AddAsync(_mapper.Map<Product>(productDto));
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


        public async Task<IActionResult> deneme()
        {


            

            var orderr = new Order
            {
                OrderDate = DateTime.UtcNow,
                CustomerId = 2,
                OrderDetails = new OrderDetail
                {
                    
                    ProductId = 2,
                    Quantity = 2
                }
            };
            
            await _orderService.AddAsync(orderr);


            return RedirectToAction("index");



        }


        public async Task<IActionResult> GetMostOrderedProducts()
        {
            var mostOrderedProducts = await _context.dailyordersummary.ToListAsync();

           
                Console.WriteLine();
            
            
            return RedirectToAction("index");
        }



    }
}
