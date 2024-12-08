using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace İleriWeb.Core.DTOs
{
    public class dailyordersummary
    {
        public DateTime orderdate { get; set; }
        public int totalorders { get; set; }
        public decimal totalrevenue { get; set; }
    }
}
