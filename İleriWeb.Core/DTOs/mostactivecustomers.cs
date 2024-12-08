using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace İleriWeb.Core.DTOs
{
    public class mostactivecustomers
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
