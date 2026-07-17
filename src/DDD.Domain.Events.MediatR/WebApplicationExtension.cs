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
    extension(WebApplication application)
    {
        /// <summary>
        /// Resolves the registered <see cref="IMediator"/> from the application's
        /// service provider and configures the shared event manager to dispatch
        /// events through it.
        /// </summary>
        public void UseMediatREventDispatcher()
        {
            EventManager.Instance.UseMediatREventDispatcher(
                application.Services.GetRequiredService<IMediator>()
            );
        }
    }
}
