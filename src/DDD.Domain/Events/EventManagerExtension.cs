namespace DDD.Domain.Events;

/// <summary>
/// Extension methods that configure an <see cref="EventManager"/> with a
/// composite dispatcher.
/// </summary>
public static class EventManagerExtension
{
    /// <summary>
    /// Configures the event manager to use a <see cref="CompositeEventDispatcher"/>,
    /// optionally populated through the supplied configuration callback.
    /// </summary>
    /// <param name="eventManager">The event manager to configure.</param>
    /// <param name="configureDispatcherFunc">
    /// An optional callback that registers the inner dispatchers of the composite.
    /// When <see langword="null"/>, a composite with no inner dispatchers is used.
    /// </param>
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
