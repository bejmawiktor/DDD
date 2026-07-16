using MediatR;

namespace DDD.Domain.Events.MediatR;

public static class EventManagerExtension
{
    extension(EventManager eventManager)
    {
        public void UseMediatREventDispatcher(IMediator mediator) =>
            eventManager.Dispatcher = new EventDispatcher(mediator);
    }
}
