using BShelf.Core;
using DDD.Domain.Events;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BShelf.API
{
    public static class WebApplicationExtension
    {
        public static void UseMediatREventDispatcher(this WebApplication application)
        {
            EventManager.Instance.UseMediatREventDispatcher(application.Services.GetRequiredService<IMediator>());
        }
    }
}