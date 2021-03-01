using System;

namespace DDD.Domain.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void Save();

        void Commit();

        void Rollback();
    }
}