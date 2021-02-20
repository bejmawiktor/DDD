using System;

namespace DDD.Model
{
    public interface IAggregateRoot<TIdentifier> : IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}