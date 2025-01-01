using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Repository.Repositories
{
	public class OrderRepository : GenericRepository<Order>, IOrderRepository
	{
		public OrderRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
		{
			
			return await _context.Order
				.Include(o => o.OrderDetails) // Sipariş detaylarını dahil et
				.ThenInclude(od => od.Product) // Ürün bilgilerini dahil et
				.Where(o => o.IdentityUserId == userId) // Kullanıcıya ait siparişleri filtrele
				.ToListAsync();
		}
	}
}
