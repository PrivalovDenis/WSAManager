using WSAManager.Core.Data.Repositories;
using WSAManager.Core.Entities;

namespace WSAManager.Data.Repositories
{
    public class OrderItemRepository : BaseRepository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(IDbContext context) : base(context)
        {
        }
    }
}
