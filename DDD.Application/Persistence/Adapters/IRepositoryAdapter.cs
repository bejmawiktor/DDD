using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Domain.Model;
using DDD.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Application.Persistence.Adapters
{
    public interface IRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier>
    : IRepository<TAggregateRoot, TIdentifier>
        where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDtoAggregateRootConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        private TDtoAggregateRootConverter Converter => new TDtoAggregateRootConverter();

        TAggregateRoot IRepository<TAggregateRoot, TIdentifier>.Get(TIdentifier identifier)
        {
            TDto aggregateRootDto = this.DtoRepository.Get(new TDtoAggregateRootConverter().ToDtoIdentifier(identifier));

            if(ReferenceEquals(aggregateRootDto, null))
            {
                return default;
            }

            return aggregateRootDto.ToDomainObject();
        }

        IEnumerable<TAggregateRoot> IRepository<TAggregateRoot, TIdentifier>.Get(Pagination pagination)
        {
            IEnumerable<TDto> aggregateRootDtos = this.DtoRepository.Get(pagination);

            if(aggregateRootDtos == null)
            {
                return null;
            }

            return this.DtoRepository.Get(pagination).Select(r => r.ToDomainObject());
        }

        void IRepository<TAggregateRoot, TIdentifier>.Add(TAggregateRoot entity)
        {
            this.DtoRepository.Add(this.Converter.ToDto(entity));
        }

        void IRepository<TAggregateRoot, TIdentifier>.Remove(TIdentifier identifier)
        {
            this.DtoRepository.Remove(this.Converter.ToDtoIdentifier(identifier));
        }

        void IRepository<TAggregateRoot, TIdentifier>.Update(TAggregateRoot entity)
        {
            this.DtoRepository.Update(this.Converter.ToDto(entity));
        }
    }
}