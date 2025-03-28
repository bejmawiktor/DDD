namespace DDD.Application.Persistence;

public interface IDtoRepository<TDto, TDtoIdentifier>
{
    TDto? Get(TDtoIdentifier identifier);

    void Add(TDto dto);

    void Remove(TDto dto);

    void Update(TDto dto);
}
