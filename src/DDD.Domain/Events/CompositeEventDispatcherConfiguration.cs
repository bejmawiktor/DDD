using System;
using System.Collections.Generic;

namespace DDD.Domain.Events;

public class CompositeEventDispatcherConfiguration
{
    protected internal List<IEventDispatcher> Dispatchers { get; }

    public CompositeEventDispatcherConfiguration()
    {
        this.Dispatchers = [];
    }

    public CompositeEventDispatcherConfiguration WithDispatcher(IEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.Dispatchers.Add(dispatcher);

        return this;
    }
}
