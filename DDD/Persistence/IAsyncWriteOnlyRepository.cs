using DDD.Model;
using System;
using System.Threading.Tasks;

namespace DDD.Persistence
{
    public interface IAsyncWriteOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task Add(TEntity entity);

        Task Remove(TEntity entity);

        Task Update(TEntity entity);
    }
}