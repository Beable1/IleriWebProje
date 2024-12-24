using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace 
	IleriWeb.Core.Repositories
{
	public interface IProductRepıository : IGenericRepository<Product>
	{

		Task<List<Product>> GetProductsWithCategory();

		Task<Product> GetProductDetailsWithIdAsync(int id);
		
	}
}
