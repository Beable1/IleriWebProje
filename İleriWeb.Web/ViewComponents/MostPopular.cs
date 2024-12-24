using IleriWeb.Core;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Services;
using IleriWeb.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IleriWeb.Web.ViewComponents
{
    public class MostPopular : ViewComponent
    {
        
        private readonly AppDbContext _context;

		public MostPopular(AppDbContext context, IProductService productService)
		{
			_context = context;
			
		}

		

        public async Task<IViewComponentResult> InvokeAsync()
        {

			var products = await _context.mostorderedproducts.ToListAsync();

			// Bu 'productname' değerlerine göre Product tablosunu filtreliyoruz
			

            
            Console.WriteLine("");

			return View(products);
        }

        [HttpGet]
        public IActionResult GetProductImage(int productId)
        {

            var product = _context.Product.Find(productId);


            if (product != null && product.ImageData != null)
            {

                return new FileContentResult(product.ImageData, "image/jpeg");
            }
            

            return (IActionResult)View();
        }


    }
}
