using DDD.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Application
{
    public interface IAsyncReadOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        Task<TDto> GetAsync(TDtoIdentifier identifier);

        Task<IEnumerable<TDto>> GetAsync(Pagination pagination = null);
    }
}