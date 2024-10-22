using System.Threading.Tasks;

namespace DDD.Domain.Common;

public interface IDispatcher<TItemBase>
{
    void Dispatch<TItem>(TItem item)
        where TItem : TItemBase;

    Task DispatchAsync<TItem>(TItem item)
        where TItem : TItemBase;
}
