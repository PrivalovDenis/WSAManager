using WSAManager.Core.Data.Repositories;
using WSAManager.Core.Entities;

namespace WSAManager.Data.Repositories
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(IDbContext context) : base(context)
        {
        }
    }
}
