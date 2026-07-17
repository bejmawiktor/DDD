using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// <see cref="IEventDispatcher"/> that delivers domain events by publishing them
/// as MediatR notifications. Internal — configured through the public extension
/// methods rather than constructed directly.
/// </summary>
/// <param name="mediator">The MediatR mediator used to publish notifications.</param>
internal class EventDispatcher(IMediator mediator) : IEventDispatcher
{
    private IMediator Mediator { get; } =
        mediator ?? throw new ArgumentNullException(nameof(mediator));

    public void Dispatch<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent =>
        this.Mediator.Publish(new EventNotification<TEvent>(@event)).GetAwaiter().GetResult();

    public Task DispatchAsync<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent =>
        this.Mediator.Publish(new EventNotification<TEvent>(@event));
}
