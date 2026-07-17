namespace DDD.Domain.Model;

/// <summary>
/// Base class for aggregate roots. Inherits identity-based equality from
/// <see cref="Entity{TIdentifier}"/> and marks the type as the consistency
/// boundary and persistence entry-point of its aggregate.
/// </summary>
/// <typeparam name="TIdentifier">
/// Type of the aggregate root identifier. Must be non-nullable and comparable
/// for equality.
/// </typeparam>
/// <param name="id">The identifier of the aggregate root. Cannot be <see langword="null"/>.</param>
public abstract class AggregateRoot<TIdentifier>(TIdentifier id)
    : Entity<TIdentifier>(id),
        IAggregateRoot<TIdentifier>
    where TIdentifier : notnull, IEquatable<TIdentifier> { }
