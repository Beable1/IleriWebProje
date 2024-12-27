using IleriWeb.Core;
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
	public class BasketService : Service<Basket>, IBasketService
	{
		private readonly IBasketRepository _basketRepository;
		private readonly IUnitOfWork _unitOfWork;

		public BasketService(IGenericRepository<Basket> repository, IUnitOfWork unitOfWork, IBasketRepository basketRepository) : base(repository, unitOfWork)
		{
			_basketRepository = basketRepository;
			_unitOfWork = unitOfWork;
		}

		public async Task ClearBasketAsync(int BasketId)
		{
			var basket = await _basketRepository.GetByIdAsync(BasketId);
			if (basket != null)
			{
				basket.Items.Clear();
				await _unitOfWork.CommitAsync();
			}
		}

		public async Task<Basket> GetBasketByUserId(int userId)
		{
			return await _basketRepository.GetBasketWithIdAsync(userId);
		}

		public async Task<Basket> GetBasketDetailsWithIdAsync(int userId)
		{
			return await _basketRepository.GetBasketDetailsWithIdAsync(userId);
		}
	}
}
