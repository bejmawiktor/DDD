namespace DDD.Application.Persistence;

/// <summary>
/// Storage abstraction expressed in terms of DTOs rather than domain objects.
/// Implemented by the concrete persistence layer (database, ORM, external
/// service) and adapted to a domain <c>IRepository</c> by the repository
/// adapters.
/// </summary>
/// <typeparam name="TDto">The DTO type being stored.</typeparam>
/// <typeparam name="TDtoIdentifier">Type of the identifier used to look up DTOs.</typeparam>
public interface IDtoRepository<TDto, in TDtoIdentifier>
{
    /// <summary>
    /// Retrieves the DTO with the given identifier.
    /// </summary>
    /// <param name="identifier">The identifier of the DTO to load.</param>
    /// <returns>The matching DTO, or <see langword="null"/> if none exists.</returns>
    TDto? Get(TDtoIdentifier identifier);

    /// <summary>
    /// Adds a new DTO to the store.
    /// </summary>
    /// <param name="dto">The DTO to add.</param>
    void Add(TDto dto);

    /// <summary>
    /// Removes a DTO from the store.
    /// </summary>
    /// <param name="dto">The DTO to remove.</param>
    void Remove(TDto dto);

    /// <summary>
    /// Persists changes to an existing DTO.
    /// </summary>
    /// <param name="dto">The DTO to update.</param>
    void Update(TDto dto);
}
