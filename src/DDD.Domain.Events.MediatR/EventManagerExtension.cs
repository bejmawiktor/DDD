using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Extension methods that wire an <see cref="EventManager"/> to a MediatR-based
/// event dispatcher.
/// </summary>
public static class EventManagerExtension
{
    /// <summary>
    /// Configures the event manager to dispatch domain events through MediatR.
    /// </summary>
    /// <param name="eventManger">The event manager to configure.</param>
    /// <param name="mediator">The MediatR mediator used to publish events.</param>
    public static void UseMediatREventDispatcher(
        this EventManager eventManger,
        IMediator mediator
    ) => eventManger.Dispatcher = new EventDispatcher(mediator);
}
