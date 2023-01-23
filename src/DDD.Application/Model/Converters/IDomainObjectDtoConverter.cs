using DDD.Domain.Model;

namespace DDD.Application.Model.Converters
{
    public interface IDomainObjectDtoConverter<TDomainObject, TDto>
        where TDomainObject : IDomainObject
        where TDto : IDomainObjectDto<TDomainObject>
    {
        TDto ToDto(TDomainObject domainObject);
    }
}