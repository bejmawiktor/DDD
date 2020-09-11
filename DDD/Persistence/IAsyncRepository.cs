using DDD.Model;
using System;

namespace DDD.Persistence
{
    public interface IAsyncRepository<TEntity, TIdentifier>
    : IAsyncWriteOnlyRepository<TEntity, TIdentifier>, IAsyncReadOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}