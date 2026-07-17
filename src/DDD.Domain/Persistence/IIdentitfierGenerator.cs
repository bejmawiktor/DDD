namespace DDD.Domain.Persistence;

/// <summary>
/// Supplies fresh, unique identifiers for newly created aggregates or entities,
/// keeping identity generation out of the domain model itself.
/// </summary>
/// <typeparam name="TIdentifier">Type of the identifier to generate.</typeparam>
public interface IIdentifierGenerator<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets a new, unique identifier. Each access yields a distinct value.
    /// </summary>
    TIdentifier NextIdentifier { get; }
}
