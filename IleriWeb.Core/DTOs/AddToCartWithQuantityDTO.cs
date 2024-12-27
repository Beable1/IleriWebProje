using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class AddToCartWithQuantityDTO
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
	}
}
