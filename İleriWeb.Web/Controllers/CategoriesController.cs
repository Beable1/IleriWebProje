﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using IleriWeb.Core;
using IleriWeb.Core.Models;
using IleriWeb.Core.Services;
using IleriWeb.Service.Services;
using IleriWeb.Core.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace IleriWeb.Web.Controllers
{
	[AllowAnonymous]
	public class CategoriesController : Controller
    {

        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;


        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }




		public async Task<IActionResult> Index()
		{
			var category = await _categoryService.GetAllAsync();

			return View(category);
		}


		public async Task<IActionResult> Products(int id)
		{
			var category = await _categoryService.GetSingleCategoryByIdWithProductsAsync(id);
			
			return View(category);
		}


	}
}
