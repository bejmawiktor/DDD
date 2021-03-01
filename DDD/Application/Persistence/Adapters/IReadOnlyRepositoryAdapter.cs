using DDD.Model;
using DDD.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Application
{
    public interface IReadOnlyRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier>
    : IReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IReadOnlyDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDtoAggregateRootConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        TAggregateRoot IReadOnlyRepository<TAggregateRoot, TIdentifier>.Get(TIdentifier identifier)
        {
            TDto aggregateRootDto = this.DtoRepository.Get(new TDtoAggregateRootConverter().ToDtoIdentifier(identifier));

            if(ReferenceEquals(aggregateRootDto, null))
            {
                return default;
            }

            return aggregateRootDto.ToDomainObject();
        }

        IEnumerable<TAggregateRoot> IReadOnlyRepository<TAggregateRoot, TIdentifier>.Get(Pagination pagination)
        {
            IEnumerable<TDto> aggregateRootDtos = this.DtoRepository.Get(pagination);

            if(aggregateRootDtos == null)
            {
                return null;
            }

            return this.DtoRepository.Get(pagination).Select(r => r.ToDomainObject());
        }
    }
}