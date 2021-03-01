using DDD.Model;
using DDD.Persistence;
using System;
using System.Threading.Tasks;

namespace DDD.Application
{
    public interface IAsyncWriteOnlyRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier>
    : IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDtoAggregateRootConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        private TDtoAggregateRootConverter Converter => new TDtoAggregateRootConverter();

        Task IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>.AddAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.AddAsync(this.Converter.ToDto(entity));
        }

        Task IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>.RemoveAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.RemoveAsync(this.Converter.ToDto(entity));
        }

        Task IAsyncWriteOnlyRepository<TAggregateRoot, TIdentifier>.UpdateAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.UpdateAsync(this.Converter.ToDto(entity));
        }
    }
}