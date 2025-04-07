using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Events;

public class CompositeEventDispatcher : ICompositeEventDispatcher
{
    protected internal List<IEventDispatcher> Dispatchers { get; }

    public CompositeEventDispatcher()
    {
        this.Dispatchers = [];
    }

    public void Add(IEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.Dispatchers.Add(dispatcher);
    }

    public void AddRange(IEnumerable<IEventDispatcher> dispatchers)
    {
        ArgumentNullException.ThrowIfNull(dispatchers);

        this.Dispatchers.AddRange(dispatchers);
    }

    public void Dispatch<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        this.Dispatchers.ForEach(dispatcher => dispatcher.Dispatch(item));

    public Task DispatchAsync<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        Parallel.ForEachAsync(
            this.Dispatchers,
            async (dispatcher, token) => await dispatcher.DispatchAsync(item)
        );
}
