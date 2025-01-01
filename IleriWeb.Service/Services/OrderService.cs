using IleriWeb.Core.Services;
using 
    IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IleriWeb.Repository.Repositories;

namespace IleriWeb.Service.Services
{
	public class OrderService : Service<Order>, IOrderService
	{
		private readonly IOrderRepository orderRepository;
		public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitOfWork, IOrderRepository orderRepository) : base(repository, unitOfWork)
		{
			this.orderRepository = orderRepository;
		}


		public async Task<Dictionary<DateTime, List<Order>>> GetUserOrdersGroupedByDateAsync(int userId)
		{
			// Kullanıcıya ait siparişleri çek
			var orders = await orderRepository.GetOrdersByUserIdAsync(userId);

			// Siparişleri tarihe göre grupla
			var groupedOrders = orders
				.GroupBy(o => o.OrderDate.Date) // Tarihe göre grupla (saat bilgisini atla)
				.ToDictionary(g => g.Key, g => g.ToList());

			return groupedOrders;
		}

	}
}
