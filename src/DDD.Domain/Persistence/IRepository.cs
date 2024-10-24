using DDD.Domain.Model;
using System;
using System.Collections.Generic;

namespace DDD.Domain.Persistence;

public interface IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    TAggregateRoot? Get(TIdentifier identifier);

    IEnumerable<TAggregateRoot> Get(Pagination? pagination = null);

    void Add(TAggregateRoot entity);

    void Update(TAggregateRoot entity);

    void Remove(TAggregateRoot entity);
}