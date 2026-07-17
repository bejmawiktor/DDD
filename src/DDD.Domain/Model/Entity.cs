namespace DDD.Domain.Model;

/// <summary>
/// Base class for domain entities. Provides identity-based equality: two
/// entities are equal when they share the same runtime type and the same
/// <see cref="Id"/>, regardless of their other attributes.
/// </summary>
/// <typeparam name="TIdentifier">
/// Type of the entity identifier. Must be non-nullable and comparable for equality.
/// </typeparam>
public abstract class Entity<TIdentifier> : IEntity<TIdentifier>, IEquatable<Entity<TIdentifier>>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the identifier that uniquely distinguishes this entity. The value
    /// can only be set by derived types and never accepts <see langword="null"/>.
    /// </summary>
    public TIdentifier Id
    {
        get;
        protected set => field = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Entity{TIdentifier}"/> class
    /// with the supplied identity.
    /// </summary>
    /// <param name="id">The identifier of the entity. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="id"/> is <see langword="null"/>.
    /// </exception>
    protected Entity(TIdentifier id)
    {
        this.Id = id;
    }

    /// <summary>
    /// Determines whether two entities represent the same identity.
    /// </summary>
    /// <param name="lhs">The left-hand entity, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand entity, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if both are <see langword="null"/> or share the same
    /// type and identifier; otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Entity<TIdentifier>? lhs, Entity<TIdentifier>? rhs) =>
        lhs is null ? rhs is null : lhs.Equals(rhs);

    /// <summary>
    /// Determines whether two entities represent different identities.
    /// </summary>
    /// <param name="lhs">The left-hand entity, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand entity, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the entities differ; otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Entity<TIdentifier>? lhs, Entity<TIdentifier>? rhs) =>
        !(lhs == rhs);

    /// <summary>
    /// Determines whether the specified object is an entity of the same type
    /// with an equal <see cref="Id"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal entity;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is Entity<TIdentifier> other
        && this.GetType() == other.GetType()
        && this.Id.Equals(other.Id);

    /// <summary>
    /// Determines whether the specified entity has the same type and identity
    /// as the current entity.
    /// </summary>
    /// <param name="other">The entity to compare with, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the entities are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(Entity<TIdentifier>? other) => this.Equals((object?)other);

    /// <summary>
    /// Returns a hash code derived from the entity's runtime type and its
    /// <see cref="Id"/>, consistent with <see cref="Equals(object?)"/>.
    /// </summary>
    /// <returns>A hash code for the current entity.</returns>
    public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Id);
}
