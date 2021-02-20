using DDD.Model;
using System;

namespace DDD.Persistence
{
    public interface IAsyncRepository<TAggregateRoot, TIdentifier>
    : IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>, IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}