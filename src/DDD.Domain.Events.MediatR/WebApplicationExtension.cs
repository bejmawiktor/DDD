using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Domain.Events.MediatR;

public static class WebApplicationExtension
{
    public static void UseMediatREventDispatcher(this WebApplication application)
    {
        EventManager.Instance.UseMediatREventDispatcher(
            application.Services.GetRequiredService<IMediator>()
        );
    }
}