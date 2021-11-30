using System;

namespace DDD.Domain.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();

        void Rollback();
    }
}