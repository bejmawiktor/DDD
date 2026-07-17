namespace DDD.Domain.Model;

/// <summary>
/// Represents a domain entity: an object whose identity is defined by its
/// <see cref="Id"/> rather than by the values of its attributes.
/// </summary>
/// <typeparam name="TIdentifier">
/// Type of the entity identifier. Must be non-nullable and comparable for
/// equality so that entities can be distinguished by their identity.
/// </typeparam>
public interface IEntity<TIdentifier> : IDomainObject
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the identifier that uniquely distinguishes this entity from every
    /// other entity of the same type.
    /// </summary>
    TIdentifier Id { get; }
}
