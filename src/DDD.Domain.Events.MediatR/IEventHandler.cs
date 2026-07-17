using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Handles a specific domain event delivered through MediatR. Implement this
/// interface to react to an event: the default explicit implementation unwraps
/// the <see cref="EventNotification{TEvent}"/> and forwards the event to
/// <see cref="Handle(TEvent, CancellationToken)"/>.
/// </summary>
/// <typeparam name="TEvent">The domain event type this handler reacts to.</typeparam>
public interface IEventHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : notnull, IEvent
{
    /// <summary>
    /// Handles the domain event.
    /// </summary>
    /// <param name="event">The domain event to handle.</param>
    /// <param name="cancellationToken">A token to observe for cancellation.</param>
    /// <returns>A task that completes when the event has been handled.</returns>
    Task Handle(TEvent @event, CancellationToken cancellationToken);

    Task INotificationHandler<EventNotification<TEvent>>.Handle(
        DDD.Domain.Events.MediatR.EventNotification<TEvent> notification,
        System.Threading.CancellationToken cancellationToken
    ) => this.Handle(notification.Event, cancellationToken);
}
