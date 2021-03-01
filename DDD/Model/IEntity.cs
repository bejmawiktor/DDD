using System;

namespace DDD.Domain.Model
{
    public interface IEntity<TIdentifier> : IDomainObject
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Id { get; }
    }
}