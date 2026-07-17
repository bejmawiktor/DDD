namespace DDD.Domain.Model;

/// <summary>
/// Represents an aggregate root: the entry-point entity of an aggregate that
/// guards its consistency boundary and is the only member other objects may
/// reference and persist directly.
/// </summary>
/// <typeparam name="TIdentifier">
/// Type of the aggregate root identifier. Must be non-nullable and comparable
/// for equality.
/// </typeparam>
public interface IAggregateRoot<TIdentifier> : IEntity<TIdentifier>
    where TIdentifier : notnull, IEquatable<TIdentifier> { }
