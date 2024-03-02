using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;
using MediatR;

namespace BShelf.Core
{
    public static class EventManagerExtension
    {
        public static void UseMediatREventDispatcher(
            this EventManager eventManger,
            IMediator mediator
        )
        {
            eventManger.EventDispatcher = new EventDispatcher(mediator);
        }
    }
}
