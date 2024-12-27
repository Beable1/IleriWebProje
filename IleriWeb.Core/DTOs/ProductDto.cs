using IleriWeb.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class ProductDto:BaseDto
	{
		public string? Name { get; set; }
		public int Stock { get; set; }
		public decimal Price { get; set; }
		public int CategoryId { get; set; }
		public ProductFeatureDto ProductFeature { get; set; }
		public string Description { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
