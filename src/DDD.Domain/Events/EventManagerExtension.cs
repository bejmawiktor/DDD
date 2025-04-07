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

        if (eventManager.Dispatcher is CompositeEventDispatcher eventManagerDispatcher)
        {
            compositeEventDispatcher = eventManagerDispatcher;
        }
        else
        {
            EventManager.Instance.Dispatcher = compositeEventDispatcher;
        }

        if (configureDispatcherFunc is not null)
        {
            configureDispatcherFunc(compositeEventDispatcher.Configuration);
        }
    }
}
