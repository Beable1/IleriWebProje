using IleriWeb.Core.Services;
using 
    IleriWeb.Core.Models;
using IleriWeb.Core.Repositories;
using IleriWeb.Core.UnitOfWorks;
using IleriWeb.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Service.Services
{
    public class OrderService : Service<Order>, IOrderService
    {
        public OrderService(IGenericRepository<Order> repository, IUnitOfWork unitOfWork) : base(repository, unitOfWork)
        {
        }
    }
}
