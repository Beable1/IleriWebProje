using IleriWeb.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Repositories
{
    public interface IOrderRepository:IGenericRepository<Order>
    {
		Task<List<Order>> GetOrdersByUserIdAsync(int userId);
		
	}
}
