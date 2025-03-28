using System.Threading.Tasks;

namespace DDD.Application.Persistence;

public interface IAsyncDtoRepository<TDto, TDtoIdentifier>
{
    Task<TDto?> GetAsync(TDtoIdentifier identifier);

    Task AddAsync(TDto dto);

    Task RemoveAsync(TDto dto);

    Task UpdateAsync(TDto dto);
}
