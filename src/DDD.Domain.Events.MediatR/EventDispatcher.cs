using MediatR;

namespace DDD.Domain.Events.MediatR;

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
