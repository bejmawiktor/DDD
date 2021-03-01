using DDD.Model;
using DDD.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Application
{
    public interface IAsyncRepositoryAdapter<TDto, TDtoIdentifier, TDtoRepository, TAggregateRootDtoConverter, TAggregateRoot, TIdentifier>
    : IAsyncRepository<TAggregateRoot, TIdentifier>
        where TDto : AggregateRootDto<TAggregateRoot, TIdentifier>
        where TDtoRepository : IAsyncReadOnlyDtoRepository<TDto, TDtoIdentifier>, IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
        where TAggregateRoot : IAggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TAggregateRootDtoConverter : IAggregateRootDtoConverter<TAggregateRoot, TIdentifier, TDto, TDtoIdentifier>, new()
    {
        protected abstract TDtoRepository DtoRepository { get; }

        private TAggregateRootDtoConverter Converter => new TAggregateRootDtoConverter();

        Task<TAggregateRoot> IAsyncReadOnlyRepository<TAggregateRoot, TIdentifier>.GetAsync(TIdentifier identifier)
        {
            return this.DtoRepository
                .GetAsync(this.Converter.ToDtoIdentifier(identifier))
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