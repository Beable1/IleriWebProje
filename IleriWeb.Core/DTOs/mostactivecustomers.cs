using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
    public class mostactivecustomers
    {
        public int customerid { get; set; }
        public string customername { get; set; }
        public int totalorders { get; set; }
        public decimal totalspent { get; set; }
    }
}
