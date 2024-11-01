using System.Threading.Tasks;

namespace DDD.Domain.Utils;

public interface IDispatcher<in TItemBase>
    where TItemBase : notnull
{
    void Dispatch<TItem>(TItem item)
        where TItem : notnull, TItemBase;

    Task DispatchAsync<TItem>(TItem item)
        where TItem : notnull, TItemBase;
}
