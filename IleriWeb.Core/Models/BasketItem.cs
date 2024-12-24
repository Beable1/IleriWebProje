using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
	public class BasketItem
	{
		public int Id { get; set; }
		public int BasketId { get; set; }
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public decimal TotalPrice { get; set; }

		
		public Basket Basket { get; set; }
		public Product Product { get; set; }
	}
}
