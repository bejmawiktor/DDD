using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Domain.Model;
using DDD.Domain.Persistence;

namespace DDD.Application.Persistence.Adapters;

/// <summary>
/// Asynchronous counterpart of
/// <see cref="IRepositoryAdapter{TDto, TDtoIdentifier, TDtoRepository, TDtoAggregateRootConverter, TAggregateRoot, TIdentifier}"/>.
/// Adapts an <see cref="IAsyncDtoRepository{TDto, TDtoIdentifier}"/> to the
/// domain-facing <see cref="IAsyncRepository{TAggregateRoot, TIdentifier}"/> by
/// converting aggregate roots to DTOs and back. Provide the backing store by
/// overriding <see cref="DtoRepository"/>.
/// </summary>
/// <typeparam name="TDto">The aggregate root DTO type used for storage.</typeparam>
/// <typeparam name="TDtoIdentifier">Type of the identifier used by the DTO store.</typeparam>
/// <typeparam name="TDtoRepository">The concrete asynchronous DTO repository type.</typeparam>
/// <typeparam name="TAggregateRootDtoConverter">
/// Converter between the aggregate root and its DTO. Must have a public
/// parameterless constructor.
/// </typeparam>
/// <typeparam name="TAggregateRoot">The domain aggregate root type.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
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
    /// <summary>
    /// Gets the underlying asynchronous DTO repository that performs the actual
    /// storage work.
    /// </summary>
    protected abstract TDtoRepository DtoRepository { get; }

    private static TAggregateRootDtoConverter Converter => new();

    async Task<TAggregateRoot?> IAsyncRepository<TAggregateRoot, TIdentifier>.GetAsync(
        TIdentifier identifier
    )
    {
        TDto? aggregateRootDto = await this.DtoRepository.GetAsync(
            Converter.ToDtoIdentifier(identifier)
        );

        return aggregateRootDto is null ? default : aggregateRootDto.ToDomainObject();
    }

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.AddAsync(TAggregateRoot entity) =>
        this.DtoRepository.AddAsync(Converter.ToDto(entity));

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.RemoveAsync(TAggregateRoot entity) =>
        this.DtoRepository.RemoveAsync(Converter.ToDto(entity));

    Task IAsyncRepository<TAggregateRoot, TIdentifier>.UpdateAsync(TAggregateRoot entity) =>
        this.DtoRepository.UpdateAsync(Converter.ToDto(entity));
}
