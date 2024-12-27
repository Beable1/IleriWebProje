using 
    IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
    public class mostorderedproduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
		public byte[] ImageData { get; set; }
        public string CategoryName { get; set; }
        public int TotalQuantityOrdered { get; set; }
        public int TotalOrders { get; set; }
        public int ProductStock { get; set; }
    }
}
