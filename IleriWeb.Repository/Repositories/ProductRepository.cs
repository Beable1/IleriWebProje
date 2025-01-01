using Microsoft.EntityFrameworkCore;
using 
	IleriWeb.Core;
using IleriWeb.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata;

namespace IleriWeb.Repository.Repositories
{
	public class ProductRepository : GenericRepository<Product>, IProductRepıository
	{
		public ProductRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<List<Product>> GetProductsWithCategory()
		{

			return await _context.Product.Include(x => x.Category).Include(x=>x.ProductFeature).ToListAsync();
		}

		public async Task<List<Product>> GetProductsWithDetails()
		{

			return await _context.Product.Include(x => x.Category).Include(x => x.ProductFeature).ToListAsync();
		}

		public async Task<Product> GetProductDetailsWithIdAsync(int id)
		{
			return await _context.Product.Include(x => x.ProductFeature).Include(x => x.Category).Where(x => x.Id==id).SingleOrDefaultAsync();
		}
	}
}
