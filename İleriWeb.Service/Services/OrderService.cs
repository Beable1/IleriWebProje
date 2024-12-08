using İleriWeb.Core.Services;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.UnitOfWorks;
using NLayer.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace İleriWeb.Service.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
