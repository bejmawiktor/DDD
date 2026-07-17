using DDD.Domain.Model;

namespace DDD.Application.Model.Converters;

/// <summary>
/// Converts an aggregate root and its identifier to their DTO representations.
/// Used by the repository adapters to translate between the domain model and
/// the persistence layer.
/// </summary>
/// <typeparam name="TAggregateRoot">The aggregate root type to convert from.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
/// <typeparam name="TDto">The aggregate root DTO type to convert to.</typeparam>
/// <typeparam name="TDtoIdentifier">Type of the identifier used by the DTO store.</typeparam>
public interface IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, out TDtoIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
    where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
{
    /// <summary>
    /// Creates the DTO representation of the given aggregate root.
    /// </summary>
    /// <param name="aggregateRoot">The aggregate root to convert.</param>
    /// <returns>The DTO representing <paramref name="aggregateRoot"/>.</returns>
    TDto ToDto(TAggregateRoot aggregateRoot);

    /// <summary>
    /// Converts a domain identifier into the identifier type used by the DTO
    /// store.
    /// </summary>
    /// <param name="identifier">The domain identifier to convert.</param>
    /// <returns>The corresponding DTO-store identifier.</returns>
    TDtoIdentifier ToDtoIdentifier(TIdentifier identifier);
}
