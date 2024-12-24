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
		public BasketService(IGenericRepository<Basket> repository, IUnitOfWork unitOfWork, IBasketRepository basketRepository) : base(repository, unitOfWork)
		{
			_basketRepository = basketRepository;
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
