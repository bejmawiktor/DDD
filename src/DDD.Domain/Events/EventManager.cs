using Utils.Disposable;

namespace DDD.Domain.Events;

/// <summary>
/// Ambient, scope-aware entry point for raising domain events. Built on the
/// Utils <see cref="ScopeHandler{TScope, TItem, THandler}"/> so that events
/// raised within an <see cref="EventsScope"/> are buffered until the scope is
/// published, and otherwise flow straight to the configured
/// <see cref="Dispatcher"/>.
/// </summary>
public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    /// <summary>
    /// Gets or sets the dispatcher that delivers events to their handlers.
    /// </summary>
    public override IDispatcher<IEvent>? Dispatcher { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="EventManager"/> class.
    /// </summary>
    public EventManager() { }

    /// <summary>
    /// Raises a domain event synchronously. If called inside an active
    /// <see cref="EventsScope"/> the event is buffered until the scope is
    /// published; otherwise it is dispatched immediately.
    /// </summary>
    /// <typeparam name="TEvent">The concrete event type.</typeparam>
    /// <param name="event">The event to raise.</param>
    public void Notify<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent => base.Handle(@event);

    /// <summary>
    /// Raises a domain event asynchronously. If called inside an active
    /// <see cref="EventsScope"/> the event is buffered until the scope is
    /// published; otherwise it is dispatched immediately.
    /// </summary>
    /// <typeparam name="TEvent">The concrete event type.</typeparam>
    /// <param name="event">The event to raise.</param>
    /// <returns>A task that completes when the event has been handled or buffered.</returns>
    public Task NotifyAsync<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent => base.HandleAsync(@event);
}
