using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Events;

public class CompositeEventDispatcher : ICompositeEventDispatcher
{
    protected internal CompositeEventDispatcherConfiguration Configuration { get; }

    public CompositeEventDispatcher()
    {
        this.Configuration = new();
    }

    public void Add(IEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.Configuration.WithDispatcher(dispatcher);
    }

    public void AddRange(IEnumerable<IEventDispatcher> dispatchers)
    {
        ArgumentNullException.ThrowIfNull(dispatchers);

        foreach (IEventDispatcher dispatcher in dispatchers)
        {
            this.Configuration.WithDispatcher(dispatcher);
        }
    }

    public void Dispatch<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        this.Configuration.Dispatchers.ForEach(dispatcher => dispatcher.Dispatch(item));

    public Task DispatchAsync<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        Parallel.ForEachAsync(
            this.Configuration.Dispatchers,
            async (dispatcher, token) => await dispatcher.DispatchAsync(item)
        );
}
