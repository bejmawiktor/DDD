using DDD.Domain.Model;

namespace DDD.Domain.Persistence;

/// <summary>
/// Abstraction over the storage of a single aggregate type, exposing the
/// synchronous operations used to load and persist aggregate roots by their
/// identifier.
/// </summary>
/// <typeparam name="TAggregateRoot">The aggregate root type this repository manages.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
public interface IRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    /// Retrieves the aggregate root with the given identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the aggregate root to load.</param>
    /// <returns>
    /// The matching aggregate root, or <see langword="null"/> if none exists.
    /// </returns>
    TAggregateRoot? Get(TIdentifier identifier);

    /// <summary>
    /// Adds a new aggregate root to the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to add.</param>
    void Add(TAggregateRoot entity);

    /// <summary>
    /// Persists changes made to an existing aggregate root.
    /// </summary>
    /// <param name="entity">The aggregate root to update.</param>
    void Update(TAggregateRoot entity);

    /// <summary>
    /// Removes an aggregate root from the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to remove.</param>
    void Remove(TAggregateRoot entity);
}
