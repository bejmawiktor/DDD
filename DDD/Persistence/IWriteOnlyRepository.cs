using DDD.Model;
using System;

namespace DDD.Persistence
{
    public interface IWriteOnlyRepository<TEntity, TIdentifier>
        where TEntity : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        void Add(TEntity entity);

        void Remove(TEntity entity);

        void Update(TEntity entity);
    }
}