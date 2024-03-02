using DDD.Domain.Model;

namespace DDD.Application.Model
{
    public interface IDomainObjectDto<out TDomainObject>
        where TDomainObject : IDomainObject
    {
        protected internal abstract TDomainObject ToDomainObject();
    }
}
