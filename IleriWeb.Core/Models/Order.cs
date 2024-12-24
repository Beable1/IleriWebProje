using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        

        public int? IdentityUserId { get; set; }
        public ApplicationUser? IdentityUser { get; set; }

        public OrderDetail OrderDetails { get; set; }
    }
}
