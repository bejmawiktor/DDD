using System.Threading.Tasks;

namespace DDD.Application
{
    public interface IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        Task AddAsync(TDto entity);

        Task RemoveAsync(TDto entity);

        Task UpdateAsync(TDto entity);
    }
}