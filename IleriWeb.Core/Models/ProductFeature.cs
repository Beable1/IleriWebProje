using IleriWeb.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
    public class ProductFeature
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Otomatik artan Id
		public int Id { get; set; }
        public string Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }
    }
}
