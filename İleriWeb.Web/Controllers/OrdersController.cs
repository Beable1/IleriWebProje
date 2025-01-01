using IleriWeb.Core.Services;
using IleriWeb.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IleriWeb.Web.Controllers
{
	
	public class OrdersController : BaseController
	{
		private readonly IOrderService orderService;

		public OrdersController(IOrderService orderService)
		{
			this.orderService = orderService;
		}

		public async Task<IActionResult> Index()
		{
			var groupedOrders = await orderService.GetUserOrdersGroupedByDateAsync(CurrentUser.Id);
			return View(groupedOrders);
		}
	}
}
