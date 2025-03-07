﻿using IleriWeb.Core.DTOs;
using IleriWeb.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 
	IleriWeb.Core.Services
{
	public interface IProductService : IService<Product>
	{
		Task<List<ProductWithCategoryDto>> GetProductsWithCategory();

		Task<Product> GetProductDetailsWithIdAsync(int id);
		Task<List<Product>> GetProductsWithDetails();

		Task UpdateStockAsync(int productId, int quantity);
	}
}
