using System;
using DDD.Domain.Model;

namespace DDD.Domain.Persistence;

public interface IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    TAggregateRoot? Get(TIdentifier identifier);

    void Add(TAggregateRoot entity);

    void Update(TAggregateRoot entity);

    void Remove(TAggregateRoot entity);
}
