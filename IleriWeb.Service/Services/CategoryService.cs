﻿using AutoMapper;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using IleriWeb.Core.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Service.Services
{
	public class CategoryService : Service<Category>, ICategoryService
	{

		private readonly ICategoryRepository _categoryRepository;
		private readonly IMapper _mapper;
		public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork,ICategoryRepository categoryRepository,IMapper mapper) : base(repository, unitOfWork)
		{
			_categoryRepository=categoryRepository;
			_mapper=mapper;
		}

		public async Task<Category> GetSingleCategoryByIdWithProductsAsync(int categoryId)
		{
			var category =await _categoryRepository.GetSingleCategoryByIdWithProductsAsync(categoryId);
			
			return category;
		}

		
	}
}
