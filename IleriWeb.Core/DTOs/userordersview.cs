using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class userordersview
	{
		public int OrderId { get; set; } // Sipariş ID'si
		public DateTime OrderDate { get; set; } // Sipariş tarihi
		public string IdentityUserId { get; set; } // Kullanıcı ID'si
		public string UserName { get; set; } // Kullanıcı adı
		public byte[] ImageData { get; set; }
		public int ProductId { get; set; } // Ürün ID'si
		public string ProductName { get; set; } // Ürün adı
		public int Quantity { get; set; } // Sipariş edilen miktar
		public decimal Price { get; set; } // Ürün fiyatı
		public decimal TotalPrice { get; set; } // Toplam tutar (Quantity * Price)
	}
}
