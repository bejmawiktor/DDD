using Microsoft.AspNetCore.Builder;

namespace DDD.Domain.Events.AspNetCore;

public static class WebApplicationExtension
{
    extension(WebApplication application)
    {
        public void UseCompositeEventDispatcher(
            Action<CompositeEventDispatcherConfiguration>? configureDispatcherFunc
        ) => EventManager.Instance.UseCompositeDispatcher(configureDispatcherFunc);

        public void UseCompositeEventDispatcher(
            Action<CompositeEventDispatcherConfiguration, IServiceProvider> configureDispatcherFunc
        ) =>
            EventManager.Instance.UseCompositeDispatcher(configuration =>
                configureDispatcherFunc(configuration, application.Services)
            );
    }
}
