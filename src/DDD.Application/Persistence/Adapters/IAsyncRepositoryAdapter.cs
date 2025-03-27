using System;
using System.Threading.Tasks;
using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Domain.Model;
using DDD.Domain.Persistence;

namespace DDD.Application.Persistence.Adapters;

public interface IAsyncRepositoryAdapter<
    TDto,
    TDtoIdentifier,
    TDtoRepository,
    TAggregateRootDtoConverter,
    TAggregateRoot,
    TIdentifier
> : IAsyncRepository<TAggregateRoot, TIdentifier>
    where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
    where TDtoRepository : IAsyncDtoRepository<TDto, TDtoIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
    where TAggregateRootDtoConverter : IAggregateRootDtoConverter<
            TAggregateRoot,
            TIdentifier,
            TDto,
            TDtoIdentifier
        >,
        new()
{
    protected abstract TDtoRepository DtoRepository { get; }

    private static TAggregateRootDtoConverter Converter => new();

    Task<TAggregateRoot?> IAsyncRepository<TAggregateRoot, TIdentifier>.GetAsync(
        TIdentifier identifier
    )
    {
        return this
            .DtoRepository.GetAsync(
                IAsyncRepositoryAdapter<
                    TDto,
                    TDtoIdentifier,
                    TDtoRepository,
                    TAggregateRootDtoConverter,
                    TAggregateRoot,
                    TIdentifier
                >.Converter.ToDtoIdentifier(identifier)
            )
            .ContinueWith(a =>
                IAsyncRepositoryAdapter<
                    TDto,
                    TDtoIdentifier,
                    TDtoRepository,
                    TAggregateRootDtoConverter,
                    TAggregateRoot,
                    TIdentifier
                >.ConvertDto(a.Result)
            );
    }

    private static TAggregateRoot? ConvertDto(TDto? aggregateRootDto) =>
        aggregateRootDto is null ? default : aggregateRootDto.ToDomainObject();

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.AddAsync(TAggregateRoot entity) =>
        this.DtoRepository.AddAsync(
            IAsyncRepositoryAdapter<
                TDto,
                TDtoIdentifier,
                TDtoRepository,
                TAggregateRootDtoConverter,
                TAggregateRoot,
                TIdentifier
            >.Converter.ToDto(entity)
        );

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.RemoveAsync(TAggregateRoot entity) =>
        this.DtoRepository.RemoveAsync(
            IAsyncRepositoryAdapter<
                TDto,
                TDtoIdentifier,
                TDtoRepository,
                TAggregateRootDtoConverter,
                TAggregateRoot,
                TIdentifier
            >.Converter.ToDto(entity)
        );

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.UpdateAsync(TAggregateRoot entity) =>
        this.DtoRepository.UpdateAsync(
            IAsyncRepositoryAdapter<
                TDto,
                TDtoIdentifier,
                TDtoRepository,
                TAggregateRootDtoConverter,
                TAggregateRoot,
                TIdentifier
            >.Converter.ToDto(entity)
        );
}
