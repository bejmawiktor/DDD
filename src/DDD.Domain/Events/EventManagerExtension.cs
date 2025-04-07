using System;

namespace DDD.Domain.Events;

public static class EventManagerExtension
{
    public static void UseCompositeDispatcher(
        this EventManager eventManager,
        Action<CompositeEventDispatcherConfiguration>? configureDispatcherFunc = null
    )
    {
        CompositeEventDispatcher compositeEventDispatcher = new();
        CompositeEventDispatcherConfiguration configuration = new();

        if (configureDispatcherFunc is not null)
        {
            configureDispatcherFunc(configuration);
            compositeEventDispatcher.AddRange(configuration.Dispatchers);
        }

        eventManager.Dispatcher = compositeEventDispatcher;
    }
}
