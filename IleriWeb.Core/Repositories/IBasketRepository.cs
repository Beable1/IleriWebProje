using IleriWeb.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.Repositories
{
    public interface IBasketRepository : IGenericRepository<Basket>
    {
        Task<Basket> GetBasketWithIdAsync(int id);

        Task<Basket> GetBasketDetailsWithIdAsync(int Id);
	}
}
