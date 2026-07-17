using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Wraps a domain event as a MediatR <see cref="INotification"/> so it can be
/// published through the MediatR pipeline to one or more
/// <see cref="IEventHandler{TEvent}"/> handlers.
/// </summary>
/// <typeparam name="TEvent">The wrapped domain event type.</typeparam>
/// <param name="event">The domain event to wrap.</param>
public class EventNotification<TEvent>(TEvent @event) : INotification
    where TEvent : notnull, DDD.Domain.Events.IEvent
{
    /// <summary>
    /// Gets the domain event carried by this notification.
    /// </summary>
    public TEvent Event { get; } = @event;
}
