using DDD.Domain.Model;

namespace DDD.Application.Model;

/// <summary>
/// A data transfer object that mirrors a domain object for persistence or
/// transport and knows how to rebuild the domain object it represents. Keeps
/// the domain model free of storage concerns in a hexagonal architecture.
/// </summary>
/// <typeparam name="TDomainObject">The domain object type this DTO maps to.</typeparam>
public interface IDomainObjectDto<out TDomainObject>
    where TDomainObject : IDomainObject
{
    /// <summary>
    /// Reconstructs the domain object represented by this DTO. Intended for use
    /// by the persistence infrastructure rather than by application code.
    /// </summary>
    /// <returns>The domain object materialized from this DTO.</returns>
    protected internal abstract TDomainObject ToDomainObject();
}
