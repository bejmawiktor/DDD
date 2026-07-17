using DDD.Domain.Model;

namespace DDD.Application.Model.Converters;

/// <summary>
/// Converts a domain object into its data transfer object. The reverse
/// direction is provided by the DTO itself through
/// <see cref="IDomainObjectDto{TDomainObject}.ToDomainObject"/>.
/// </summary>
/// <typeparam name="TDomainObject">The domain object type to convert from.</typeparam>
/// <typeparam name="TDto">The DTO type to convert to.</typeparam>
public interface IDomainObjectDtoConverter<in TDomainObject, TDto>
    where TDomainObject : IDomainObject
    where TDto : IDomainObjectDto<TDomainObject>
{
    /// <summary>
    /// Creates the DTO representation of the given domain object.
    /// </summary>
    /// <param name="domainObject">The domain object to convert.</param>
    /// <returns>The DTO representing <paramref name="domainObject"/>.</returns>
    TDto ToDto(TDomainObject domainObject);
}
