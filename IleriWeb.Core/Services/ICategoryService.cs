﻿using IleriWeb.Core.DTOs;
using 
	IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Services
{
	public interface ICategoryService:IService<Category>
	{
		public Task<Category>GetSingleCategoryByIdWithProductsAsync(int categoryId);
	}
}
