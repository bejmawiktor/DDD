using DDD.Model;
using System;

namespace DDD.Persistence
{
    public interface IRepository<TEntity, TIdentifier> 
    : IWriteOnlyRepository<TEntity, TIdentifier>, IReadOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}