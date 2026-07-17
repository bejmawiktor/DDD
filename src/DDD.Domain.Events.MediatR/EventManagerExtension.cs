using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Extension methods that wire an <see cref="EventManager"/> to a MediatR-based
/// event dispatcher.
/// </summary>
public static class EventManagerExtension
{
    extension(EventManager eventManager)
    {
        /// <summary>
        /// Configures the event manager to dispatch domain events through MediatR.
        /// </summary>
        /// <param name="mediator">The MediatR mediator used to publish events.</param>
        public void UseMediatREventDispatcher(IMediator mediator) =>
            eventManager.Dispatcher = new EventDispatcher(mediator);
    }
}
