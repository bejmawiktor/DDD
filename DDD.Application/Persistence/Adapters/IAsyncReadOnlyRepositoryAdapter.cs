using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Domain.Model;
using DDD.Domain.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Application.Persistence.Adapters
{
    public interface IAsyncReadOnlyRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier>
    : IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IAsyncReadOnlyDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TDtoAggregateRootConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        Task<TAggregateRoot> IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>.GetAsync(TIdentifier identifier)
        {
            return this.DtoRepository
                .GetAsync(new TDtoAggregateRootConverter().ToDtoIdentifier(identifier))
                .ContinueWith(a => this.ConvertDto(a.Result));
        }

        private TAggregateRoot ConvertDto(TDto aggregateRootDto)
        {
            if(ReferenceEquals(aggregateRootDto, null))
            {
                return default;
            }

            return aggregateRootDto.ToDomainObject();
        }

        Task<IEnumerable<TAggregateRoot>> IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>.GetAsync(Pagination pagination)
        {
            return this.DtoRepository
                .GetAsync(pagination)
                .ContinueWith(r => r.Result?.Select(r => r.ToDomainObject()));
        }
    }
}