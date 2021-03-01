using DDD.Model;
using System;

namespace DDD.Application
{
    public abstract class AggregateRootDto<TAggregateRoot, TIdentifier> : DomainObjectDto<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
    }
}