using WSAManager.Core.Data.Repositories;
using WSAManager.Core.Entities;
using System;
using System.Threading.Tasks;

namespace WSAManager.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        
        void BeginTransaction();
        
        int Commit();
        
        Task<int> CommitAsync();

        void Rollback();

        void Dispose(bool disposing);

    }
}
