using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
    public class productstockstatus
    {
        public int productid { get; set; }
        public string productname { get; set; }
        public int unitsinstock { get; set; }
        public string stockstatus { get; set; }
    }

}
