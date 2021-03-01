using DDD.Model;

namespace DDD.Application
{
    public interface IDomainObjectDtoConverter<TDomainObject, TDto>
        where TDomainObject : IDomainObject
        where TDto : DomainObjectDto<TDomainObject>
    {
        TDto ToDto(TDomainObject domainObject);
    }
}