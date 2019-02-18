using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WSAManager.Core.Data;
using WSAManager.Core.Entities;
using WSAManager.Core.Services;
using WSAManager.Data;

namespace WSAManager.Services.Services
{
    public class OrderItemService : BaseService<OrderItem>, IOrderItemService
    {
        public OrderItemService(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}
