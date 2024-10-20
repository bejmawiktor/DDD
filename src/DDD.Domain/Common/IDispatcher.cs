using System.Threading.Tasks;

namespace DDD.Domain.Common;

internal interface IDispatcher
{
    void Dispatch<TItem>(TItem item);

    Task DispatchAsync<TItem>(TItem item);
}
