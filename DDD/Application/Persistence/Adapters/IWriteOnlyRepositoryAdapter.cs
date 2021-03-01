using DDD.Model;
using DDD.Persistence;
using System;

namespace DDD.Application
{
    public interface IWriteOnlyRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier>
    : IWriteOnlyRepository<TAggregateRoot, TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IWriteOnlyDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDtoAggregateRootConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        void IWriteOnlyRepository<TAggregateRoot, TIdentifier>.Add(TAggregateRoot entity)
        {
            this.DtoRepository.Add(new TDtoAggregateRootConverter().ToDto(entity));
        }

        void IWriteOnlyRepository<TAggregateRoot, TIdentifier>.Remove(TAggregateRoot entity)
        {
            this.DtoRepository.Remove(new TDtoAggregateRootConverter().ToDto(entity));
        }

        void IWriteOnlyRepository<TAggregateRoot, TIdentifier>.Update(TAggregateRoot entity)
        {
            this.DtoRepository.Update(new TDtoAggregateRootConverter().ToDto(entity));
        }
    }
}