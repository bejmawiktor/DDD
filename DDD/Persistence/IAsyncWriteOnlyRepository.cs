using DDD.Model;
using System;
using System.Threading.Tasks;

namespace DDD.Persistence
{
    public interface IAsyncWriteOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task AddAsync(TEntity entity);

        Task RemoveAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);
    }
}