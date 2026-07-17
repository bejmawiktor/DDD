using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Extension methods that configure the ambient <see cref="EventManager"/> of a
/// <see cref="WebApplication"/> to dispatch domain events through MediatR.
/// </summary>
public static class WebApplicationExtension
{
    /// <summary>
    /// Resolves the registered <see cref="IMediator"/> from the application's
    /// service provider and configures the shared event manager to dispatch
    /// events through it.
    /// </summary>
    /// <param name="application">The web application whose event manager is configured.</param>
    public static void UseMediatREventDispatcher(this WebApplication application)
    {
        EventManager.Instance.UseMediatREventDispatcher(
            application.Services.GetRequiredService<IMediator>()
        );
    }
}
