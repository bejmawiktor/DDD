using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Application.Persistence
{
    public interface IAsyncDtoRepository<TDto, TDtoIdentifier>
    {
        Task<TDto> GetAsync(TDtoIdentifier identifier);

        Task<IEnumerable<TDto>> GetAsync(Pagination pagination = null);

        Task AddAsync(TDto entity);

        Task RemoveAsync(TDtoIdentifier identifier);

        Task UpdateAsync(TDto entity);
    }
}