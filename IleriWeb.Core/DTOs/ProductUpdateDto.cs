using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class ProductUpdateDto:BaseDto
	{
        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        public string? Name { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Stok miktarı 1'den büyük olmalıdır.")]
        public int Stock { get; set; }
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0.01'den büyük olmalıdır.")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public int CategoryId { get; set; }


        [StringLength(900, ErrorMessage = "Açıklama en fazla 900 karakter olabilir.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Ürün resmi zorunludur.")]
        public IFormFile imageFile { get; set; }

    }
}
