using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        // Ekstra özellikler ekleyebilirsiniz
        public string FullName { get; set; }
        public List<Order> Orders { get; set; }
        
		public DateTime CreatedAt { get; set; }
    }
}
