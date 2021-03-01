using DDD.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Persistence
{
    public interface IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task<TAggregateRoot> GetAsync(TIdentifier identifier);

        Task<IEnumerable<TAggregateRoot>> GetAsync(Pagination pagination = null);
    }
}