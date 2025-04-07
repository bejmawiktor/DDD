using Microsoft.AspNetCore.Builder;

namespace DDD.Domain.Events.AspNetCore;

public static class WebApplicationExtension
{
    public static void UseCompositeEventDispatcher(
        this WebApplication application,
        Action<CompositeEventDispatcherConfiguration>? configureDispatcherFunc
    ) => EventManager.Instance.UseCompositeDispatcher(configureDispatcherFunc);

    public static void UseCompositeEventDispatcher(
        this WebApplication application,
        Action<IServiceProvider, CompositeEventDispatcherConfiguration> configureDispatcherFunc
    ) =>
        EventManager.Instance.UseCompositeDispatcher(configuration =>
            configureDispatcherFunc(application.Services, configuration)
        );
}
