using DDD.Model;
using System;

namespace DDD.Application
{
    public interface IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
    {
        TDto ToDto(TAggregateRoot aggregateRoot);

        TDtoIdentifier ToDtoIdentifier(TIdentifier identifier);
    }
}