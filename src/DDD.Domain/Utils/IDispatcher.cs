using System.Threading.Tasks;

namespace DDD.Domain.Utils;

public interface IDispatcher<in TItemBase>
{
    void Dispatch<TItem>(TItem item)
        where TItem : TItemBase;

    Task DispatchAsync<TItem>(TItem item)
        where TItem : TItemBase;
}
