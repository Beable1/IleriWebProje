using Microsoft.EntityFrameworkCore;
using 
	IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Repository.Repositories
{
	public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
	{
		public CategoryRepository(AppDbContext context) : base(context)
		{
		}

		public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
		{
			return await _context.Categories.Include(x => x.Products).Where(x => x.Id == categoryId).SingleOrDefaultAsync();
		}
	}
}
