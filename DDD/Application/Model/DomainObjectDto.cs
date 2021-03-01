using DDD.Model;

namespace DDD.Application
{
    public abstract class DomainObjectDto<TDomainObject>
        where TDomainObject : IDomainObject
    {
        protected internal abstract TDomainObject ToDomainObject();
    }
}