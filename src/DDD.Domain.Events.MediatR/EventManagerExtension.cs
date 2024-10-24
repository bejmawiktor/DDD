using MediatR;

namespace DDD.Domain.Events.MediatR;

public static class EventManagerExtension
{
    public static void UseMediatREventDispatcher(
        this EventManager eventManger,
        IMediator mediator
    ) => eventManger.Dispatcher = new EventDispatcher(mediator);
}