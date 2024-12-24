using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Repository.Repositories
{
	public class BasketRepository : GenericRepository<Basket>, IBasketRepository
	{
		public BasketRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<Basket> GetBasketDetailsWithIdAsync(int Id)
		{
			var basket = await _context.Baskets
							.Where(x => x.UserId == Id) 
							.Include(x => x.Items)   
								.ThenInclude(x => x.Product)  
							.SingleOrDefaultAsync();
			return basket;
		}

		public async Task<Basket> GetBasketWithIdAsync(int id)
		{
			return await _context.Baskets.Where(x => x.UserId == id).SingleOrDefaultAsync();
		}
	}


}
