using Microsoft.AspNetCore.Builder;

namespace DDD.Domain.Events.AspNetCore;

/// <summary>
/// Extension methods that configure the ambient <see cref="EventManager"/> of a
/// <see cref="WebApplication"/> with a composite event dispatcher.
/// </summary>
public static class WebApplicationExtension
{
    /// <summary>
    /// Configures the shared event manager to use a composite dispatcher built
    /// by the supplied configuration callback.
    /// </summary>
    /// <param name="application">The web application whose event manager is configured.</param>
    /// <param name="configureDispatcherFunc">
    /// A callback that registers the inner dispatchers, or <see langword="null"/>
    /// for a composite with no inner dispatchers.
    /// </param>
    public static void UseCompositeEventDispatcher(
        this WebApplication application,
        Action<CompositeEventDispatcherConfiguration>? configureDispatcherFunc
    ) => EventManager.Instance.UseCompositeDispatcher(configureDispatcherFunc);

    public static void UseCompositeEventDispatcher(
        this WebApplication application,
        Action<CompositeEventDispatcherConfiguration, IServiceProvider> configureDispatcherFunc
    ) =>
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configureDispatcherFunc(configuration, application.Services)
        );
}
