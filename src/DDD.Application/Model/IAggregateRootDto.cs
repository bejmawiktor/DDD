using DDD.Domain.Model;

namespace DDD.Application.Model;

/// <summary>
/// A data transfer object representing an aggregate root. Specializes
/// <see cref="IDomainObjectDto{TDomainObject}"/> for aggregate roots so it can
/// be stored and rehydrated through the persistence adapters.
/// </summary>
/// <typeparam name="TAggregateRoot">The aggregate root type this DTO maps to.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
public interface IAggregateRootDto<out TAggregateRoot, TIdentifier>
    : IDomainObjectDto<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier> { }
