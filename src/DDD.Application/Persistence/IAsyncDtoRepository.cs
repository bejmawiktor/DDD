namespace DDD.Application.Persistence;

/// <summary>
/// Asynchronous counterpart of <see cref="IDtoRepository{TDto, TDtoIdentifier}"/>,
/// expressing DTO storage through awaitable operations.
/// </summary>
/// <typeparam name="TDto">The DTO type being stored.</typeparam>
/// <typeparam name="TDtoIdentifier">Type of the identifier used to look up DTOs.</typeparam>
public interface IAsyncDtoRepository<TDto, TDtoIdentifier>
{
    /// <summary>
    /// Asynchronously retrieves the DTO with the given identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the DTO to load.</param>
    /// <returns>
    /// A task producing the matching DTO, or <see langword="null"/> if none exists.
    /// </returns>
    Task<TDto?> GetAsync(TDtoIdentifier identifier);

    /// <summary>
    /// Asynchronously adds a new DTO to the store.
    /// </summary>
    /// <param name="dto">The DTO to add.</param>
    /// <returns>A task that completes when the DTO has been added.</returns>
    Task AddAsync(TDto dto);

    /// <summary>
    /// Asynchronously removes a DTO from the store.
    /// </summary>
    /// <param name="dto">The DTO to remove.</param>
    /// <returns>A task that completes when the DTO has been removed.</returns>
    Task RemoveAsync(TDto dto);

    /// <summary>
    /// Asynchronously persists changes to an existing DTO.
    /// </summary>
    /// <param name="dto">The DTO to update.</param>
    /// <returns>A task that completes when the DTO has been updated.</returns>
    Task UpdateAsync(TDto dto);
}
