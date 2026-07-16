namespace DDD.Domain.Events;

public static class EventManagerExtension
{
    extension(EventManager eventManager)
    {
        public void UseCompositeDispatcher(
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
}
