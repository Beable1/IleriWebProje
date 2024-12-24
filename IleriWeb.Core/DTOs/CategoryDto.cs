using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
	public class CategoryDto:BaseDto
    {
        public string Name { get; set; }
        public IFormFile imageFile { get; set; }
    }
}
