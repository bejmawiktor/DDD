using DDD.Domain.Model;
using System;

namespace DDD.Application.Model;

public interface IAggregateRootDto<out TAggregateRoot, TIdentifier>
    : IDomainObjectDto<TAggregateRoot>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{ }