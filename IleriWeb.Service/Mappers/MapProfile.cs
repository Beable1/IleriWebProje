using AutoMapper;
using IleriWeb.Core;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Service.Mappers
{
	public class MapProfile:Profile
	{
        public MapProfile()
        {
			CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
			CreateMap<ProductFeature, ProductFeatureDto>().ReverseMap();
			CreateMap<ProductUpdateDto, Product>();
			CreateMap<Product, ProductWithCategoryDto>().ReverseMap();
			CreateMap<Category,CategoryWithProductsDto>().ReverseMap();
		}
    }
}
