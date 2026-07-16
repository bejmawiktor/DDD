using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DDD.Domain.Events.MediatR;

public static class WebApplicationExtension
{
    extension(WebApplication application)
    {
        public void UseMediatREventDispatcher()
        {
            EventManager.Instance.UseMediatREventDispatcher(
                application.Services.GetRequiredService<IMediator>()
            );
        }
    }
}
