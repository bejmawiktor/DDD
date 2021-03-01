using DDD.Domain.Model;
using System;

namespace DDD.Domain.Persistence
{
    public interface IRepository<TAggregateRoot, TIdentifier>
    : IWriteOnlyRepository<TAggregateRoot, TIdentifier>, IReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}