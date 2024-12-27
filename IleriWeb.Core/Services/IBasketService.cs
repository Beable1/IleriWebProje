using IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Services
{
	public interface IBasketService: IService<Basket>
	{
		Task<Basket> GetBasketByUserId(int userId);
		Task<Basket> GetBasketDetailsWithIdAsync(int userId);
		Task ClearBasketAsync(int BasketId);
	}
}
