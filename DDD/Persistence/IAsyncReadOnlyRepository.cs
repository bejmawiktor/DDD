using DDD.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Persistence
{
    public interface IAsyncReadOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        Task<TEntity> Get(TIdentifier identifier);

        Task<IEnumerable<TEntity>> Get(Pagination pagination = null);
    }
}