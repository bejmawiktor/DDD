using System.Threading.Tasks;

namespace DDD.Application.Persistence
{
    public interface IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        Task AddAsync(TDto entity);

        Task RemoveAsync(TDto entity);

        Task UpdateAsync(TDto entity);
    }
}