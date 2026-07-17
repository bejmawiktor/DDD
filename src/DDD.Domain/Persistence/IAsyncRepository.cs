using DDD.Domain.Model;

namespace DDD.Domain.Persistence;

/// <summary>
/// Asynchronous counterpart of <see cref="IRepository{TAggregateRoot, TIdentifier}"/>,
/// exposing awaitable operations to load and persist aggregate roots by their
/// identifier.
/// </summary>
/// <typeparam name="TAggregateRoot">The aggregate root type this repository manages.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
public interface IAsyncRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    /// Asynchronously retrieves the aggregate root with the given identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the aggregate root to load.</param>
    /// <returns>
    /// A task producing the matching aggregate root, or <see langword="null"/> if
    /// none exists.
    /// </returns>
    Task<TAggregateRoot?> GetAsync(TIdentifier identifier);

    /// <summary>
    /// Asynchronously adds a new aggregate root to the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to add.</param>
    /// <returns>A task that completes when the aggregate root has been added.</returns>
    Task AddAsync(TAggregateRoot entity);

    /// <summary>
    /// Asynchronously persists changes made to an existing aggregate root.
    /// </summary>
    /// <param name="entity">The aggregate root to update.</param>
    /// <returns>A task that completes when the aggregate root has been updated.</returns>
    Task UpdateAsync(TAggregateRoot entity);

    /// <summary>
    /// Asynchronously removes an aggregate root from the repository.
    /// </summary>
    /// <param name="entity">The aggregate root to remove.</param>
    /// <returns>A task that completes when the aggregate root has been removed.</returns>
    Task RemoveAsync(TAggregateRoot entity);
}
