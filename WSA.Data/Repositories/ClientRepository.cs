using WSAManager.Core.Data.Repositories;
using WSAManager.Core.Entities;

namespace WSAManager.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(IDbContext context) : base (context)
        {
        }
    }
}
