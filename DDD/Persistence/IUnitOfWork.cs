using System;

namespace DDD.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void Save();

        void Commit();

        void Rollback();
    }
}