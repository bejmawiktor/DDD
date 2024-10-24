using System.Threading.Tasks;

namespace DDD.Domain.Common;

public interface IDispatcher
{
    void Dispatch<TItem>(TItem item);

    Task DispatchAsync<TItem>(TItem item);
}