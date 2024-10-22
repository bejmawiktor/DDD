using System;
using DDD.Domain.Model;

namespace DDD.Application.Model
{
    public interface IAggregateRootDto<out TAggregateRoot, TIdentifier>
        : IDomainObjectDto<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier> { }
}
