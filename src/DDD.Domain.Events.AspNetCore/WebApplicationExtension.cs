using Microsoft.AspNetCore.Builder;

namespace DDD.Domain.Events.AspNetCore;

/// <summary>
/// Extension methods that configure the ambient <see cref="EventManager"/> of a
/// <see cref="WebApplication"/> with a composite event dispatcher.
/// </summary>
public static class WebApplicationExtension
{
    extension(WebApplication application)
    {
        /// <summary>
        /// Configures the shared event manager to use a composite dispatcher built
        /// by the supplied configuration callback.
        /// </summary>
        /// <param name="configureDispatcherFunc">
        /// A callback that registers the inner dispatchers, or <see langword="null"/>
        /// for a composite with no inner dispatchers.
        /// </param>
        public void UseCompositeEventDispatcher(
            Action<CompositeEventDispatcherConfiguration>? configureDispatcherFunc
        ) => EventManager.Instance.UseCompositeDispatcher(configureDispatcherFunc);

        /// <summary>
        /// Configures the shared event manager to use a composite dispatcher, giving
        /// the configuration callback access to the application's
        /// <see cref="IServiceProvider"/> so dispatchers can resolve their
        /// dependencies.
        /// </summary>
        /// <param name="configureDispatcherFunc">
        /// A callback that registers the inner dispatchers using the supplied service
        /// provider.
        /// </param>
        public void UseCompositeEventDispatcher(
            Action<CompositeEventDispatcherConfiguration, IServiceProvider> configureDispatcherFunc
        ) =>
            EventManager.Instance.UseCompositeDispatcher(configuration =>
                configureDispatcherFunc(configuration, application.Services)
            );
    }
}
