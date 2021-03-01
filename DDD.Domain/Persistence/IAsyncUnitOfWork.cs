using System.Threading.Tasks;

namespace DDD.Domain.Persistence
{
    public interface IAsyncUnitOfWork
    {
        Task BeginTransactionAsync();

        Task SaveAsync();

        Task CommitAsync();

        Task RollbackAsync();
    }
}