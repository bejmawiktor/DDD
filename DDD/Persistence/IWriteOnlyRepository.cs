using DDD.Domain.Model;
using System;

namespace DDD.Domain.Persistence
{
    public interface IWriteOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        void Add(TAggregateRoot entity);

        void Remove(TAggregateRoot entity);

        void Update(TAggregateRoot entity);
    }
}