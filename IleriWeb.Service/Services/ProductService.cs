using AutoMapper;
using IleriWeb.Core;
using IleriWeb.Core.DTOs;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.Services;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Repository.Repositories;
using IleriWeb.Service.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Service.Services
{
	public class ProductService : Service<Product>, IProductService
	{
		private readonly IProductRepıository _productRepository;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepıository productRepository) : base(repository, unitOfWork)
		{
			_mapper = mapper;
			_productRepository = productRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Product> GetProductDetailsWithIdAsync(int id)
		{
			var product = await _productRepository.GetProductDetailsWithIdAsync(id);
			return product;
		}

		public async Task<List<ProductWithCategoryDto>> GetProductsWithCategory()
		{
			var products = await _productRepository.GetProductsWithCategory();

			var productsDto = _mapper.Map<List<ProductWithCategoryDto>>(products);
			return productsDto;

		}

		public async Task UpdateStockAsync(int productId, int quantity)
		{
			var product = await _productRepository.GetByIdAsync(productId);
			if (product == null)
			{
				throw new NotFoundException("Ürün bulunamadı.");
			}

			if (product.Stock < quantity)
			{
				throw new InvalidOperationException("Yetersiz stok.");
			}

			product.Stock -= quantity;
			_productRepository.Update(product);
			await _unitOfWork.CommitAsync();
		}



	}
}
