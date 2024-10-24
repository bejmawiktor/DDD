using MediatR;

namespace DDD.Domain.Events.MediatR;

internal class EventDispatcher : IEventDispatcher
{
    private IMediator Mediator { get; }

    public EventDispatcher(IMediator mediator)
    {
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public void Dispatch<TEvent>(TEvent @event) =>
        this.Mediator.Publish(@event).GetAwaiter().GetResult();

    public Task DispatchAsync<TEvent>(TEvent @event) => this.Mediator.Publish(@event);
}