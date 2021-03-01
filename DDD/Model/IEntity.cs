using System;

namespace DDD.Model
{
    public interface IEntity<TIdentifier> : IDomainObject
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Id { get; }
    }
}