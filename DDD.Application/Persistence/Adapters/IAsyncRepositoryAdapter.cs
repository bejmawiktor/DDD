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
    public interface IAsyncRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TAggregateRootDtoConverter, TAggregateRoot, TIdentifier>
    : IAsyncRepository<TAggregateRoot, TIdentifier>
        where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IAsyncDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TAggregateRootDtoConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        private TAggregateRootDtoConverter Converter => new TAggregateRootDtoConverter();

        Task<TAggregateRoot?> IAsyncRepository<TAggregateRoot, TIdentifier>.GetAsync(TIdentifier identifier)
        {
            return this.DtoRepository
                .GetAsync(this.Converter.ToDtoIdentifier(identifier))
                .ContinueWith(a => this.ConvertDto(a.Result));
        }

        private TAggregateRoot? ConvertDto(TDto? aggregateRootDto)
        {
            if(ReferenceEquals(aggregateRootDto, null))
            {
                return default;
            }

            return aggregateRootDto.ToDomainObject();
        }

        Task<IEnumerable<TAggregateRoot>> IAsyncRepository<TAggregateRoot, TIdentifier>.GetAsync(Pagination? pagination)
        {
            return this.DtoRepository
                .GetAsync(pagination)
                .ContinueWith(r => r.Result?.Select(r => r.ToDomainObject()) ?? Enumerable.Empty<TAggregateRoot>());
        }

        Task IAsyncRepository<TAggregateRoot, TIdentifier>.AddAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.AddAsync(this.Converter.ToDto(entity));
        }

        Task IAsyncRepository<TAggregateRoot, TIdentifier>.RemoveAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.RemoveAsync(this.Converter.ToDto(entity));
        }

        Task IAsyncRepository<TAggregateRoot, TIdentifier>.UpdateAsync(TAggregateRoot entity)
        {
            return this.DtoRepository.UpdateAsync(this.Converter.ToDto(entity));
        }
    }
}