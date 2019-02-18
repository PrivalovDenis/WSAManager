using WSAManager.Core.Data.Repositories;
using WSAManager.Core.Entities;

namespace WSAManager.Data.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(IDbContext context) : base(context)
        {
        }
    }
}
