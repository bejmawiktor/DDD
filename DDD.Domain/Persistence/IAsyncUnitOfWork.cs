using System.Threading.Tasks;

namespace DDD.Domain.Persistence
{
    public interface IAsyncUnitOfWork
    {
        Task CommitAsync();

        Task RollbackAsync();
    }
}