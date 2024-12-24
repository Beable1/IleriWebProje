using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
	public class Basket
	{
		public int Id { get; set; }
		public int UserId { get; set; } 
		public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

		
		public List<BasketItem> Items { get; set; } 
	}
}
