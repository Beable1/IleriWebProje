using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class ProductFeatureDto
	{
		[Required(ErrorMessage = "Renk zorunludur.")]
		public string Color { get; set; }

		public int Width { get; set; }
		public int Height { get; set; }

		public int ProductId { get; set; }
	}
}
