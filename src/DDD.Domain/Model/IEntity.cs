using System;

namespace DDD.Domain.Model;

public interface IEntity<TIdentifier> : IDomainObject
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    TIdentifier Id { get; }
}
