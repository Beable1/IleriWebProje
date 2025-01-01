using 
    IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Services
{
    public interface IOrderService:IService<Order>
    {
		Task<Dictionary<DateTime, List<Order>>> GetUserOrdersGroupedByDateAsync(int userId);
		
	}
}
