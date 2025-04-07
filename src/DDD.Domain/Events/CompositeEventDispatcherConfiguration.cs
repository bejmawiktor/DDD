using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Events;

public class CompositeEventDispatcherConfiguration
{
    protected internal List<IEventDispatcher> Dispatchers { get; }

    public CompositeEventDispatcherConfiguration()
    {
        this.Dispatchers = [];
    }

    public CompositeEventDispatcherConfiguration WithDispatcher(IEventDispatcher eventDispatcher)
    {
        this.Dispatchers.Add(eventDispatcher);

        return this;
    }
}
