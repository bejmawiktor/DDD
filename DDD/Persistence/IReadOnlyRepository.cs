using DDD.Model;
using System;
using System.Collections.Generic;

namespace DDD.Persistence
{
    public interface IReadOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        TEntity Get(TIdentifier identifier);

        IEnumerable<TEntity> Get(Pagination pagination = null);
    }
}