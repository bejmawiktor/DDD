using DDD.Domain.Model;
using System;

namespace DDD.Application.Model
{
    public abstract class AggregateRootDto<TAggregateRoot, TIdentifier> : DomainObjectDto<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}