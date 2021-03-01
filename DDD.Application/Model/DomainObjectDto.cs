using DDD.Domain.Model;

namespace DDD.Application.Model
{
    public abstract class DomainObjectDto<TDomainObject>
        where TDomainObject : IDomainObject
    {
        protected internal abstract TDomainObject ToDomainObject();
    }
}