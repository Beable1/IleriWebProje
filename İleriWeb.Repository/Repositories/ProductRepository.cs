using Microsoft.EntityFrameworkCore;
using NLayer.Core;
using NLayer.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Repository.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepıository
	{
		public ProductRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<List<Product>> GetProductsWithCategory()
		{
			
			return await _context.Product.Include(x => x.Category).ToListAsync();
		}
	}
}
