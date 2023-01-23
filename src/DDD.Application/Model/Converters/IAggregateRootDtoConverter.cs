using DDD.Domain.Model;
using System;

namespace DDD.Application.Model.Converters
{
    public interface IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
    {
        TDto ToDto(TAggregateRoot aggregateRoot);

        TDtoIdentifier ToDtoIdentifier(TIdentifier identifier);
    }
}