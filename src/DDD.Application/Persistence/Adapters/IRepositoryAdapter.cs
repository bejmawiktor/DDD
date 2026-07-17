using DDD.Application.Model;
using DDD.Application.Model.Converters;
using DDD.Domain.Model;
using DDD.Domain.Persistence;

namespace DDD.Application.Persistence.Adapters;

/// <summary>
/// Adapts a DTO-based <see cref="IDtoRepository{TDto, TDtoIdentifier}"/> to the
/// domain-facing <see cref="IRepository{TAggregateRoot, TIdentifier}"/>. It
/// implements the domain repository operations by converting aggregate roots to
/// DTOs (and back) through the supplied converter, so the domain layer stays
/// unaware of the storage representation. Provide the backing store by
/// overriding <see cref="DtoRepository"/>.
/// </summary>
/// <typeparam name="TDto">The aggregate root DTO type used for storage.</typeparam>
/// <typeparam name="TDtoIdentifier">Type of the identifier used by the DTO store.</typeparam>
/// <typeparam name="TDtoRepository">The concrete DTO repository type.</typeparam>
/// <typeparam name="TDtoAggregateRootConverter">
/// Converter between the aggregate root and its DTO. Must have a public
/// parameterless constructor.
/// </typeparam>
/// <typeparam name="TAggregateRoot">The domain aggregate root type.</typeparam>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
public interface IRepositoryAdapter<
    TDto,
    TDtoIdentifier,
    TDtoRepository,
    TDtoAggregateRootConverter,
    TAggregateRoot,
    TIdentifier
> : IRepository<TAggregateRoot, TIdentifier>
    where TDto : IAggregateRootDto<TAggregateRoot, TIdentifier>
    where TDtoRepository : IDtoRepository<TDto, TDtoIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
    where TDtoAggregateRootConverter : IAggregateRootDtoConverter<
            TAggregateRoot,
            TIdentifier,
            TDto,
            TDtoIdentifier
        >,
        new()
{
    /// <summary>
    /// Gets the underlying DTO repository that performs the actual storage work.
    /// </summary>
    protected abstract TDtoRepository DtoRepository { get; }

    private static TDtoAggregateRootConverter Converter => new();

    TAggregateRoot? IRepository<TAggregateRoot, TIdentifier>.Get(TIdentifier identifier)
    {
        TDto? aggregateRootDto = this.DtoRepository.Get(Converter.ToDtoIdentifier(identifier));

        return aggregateRootDto is null ? default : aggregateRootDto.ToDomainObject();
    }

    void IRepository<TAggregateRoot, TIdentifier>.Add(TAggregateRoot entity) =>
        this.DtoRepository.Add(Converter.ToDto(entity));

    void IRepository<TAggregateRoot, TIdentifier>.Remove(TAggregateRoot entity) =>
        this.DtoRepository.Remove(Converter.ToDto(entity));

    void IRepository<TAggregateRoot, TIdentifier>.Update(TAggregateRoot entity) =>
        this.DtoRepository.Update(Converter.ToDto(entity));
}
