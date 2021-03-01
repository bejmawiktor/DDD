using DDD.Domain.Model;
using System;
using System.Threading.Tasks;

namespace DDD.Domain.Persistence
{
    public interface IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task AddAsync(TAggregateRoot entity);

        Task RemoveAsync(TAggregateRoot entity);

        Task UpdateAsync(TAggregateRoot entity);
    }
}