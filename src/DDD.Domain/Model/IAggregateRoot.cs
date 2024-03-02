using System;

namespace DDD.Domain.Model
{
    public interface IAggregateRoot<TIdentifier> : IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier> { }
}
