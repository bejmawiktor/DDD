using System.Threading.Tasks;
using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    public override IDispatcher<IEvent>? Dispatcher { get; set; }

    public EventManager() { }

    public void Notify<TEvent>(TEvent @event)
        where TEvent : IEvent => base.Handle(@event);

    public Task NotifyAsync<TEvent>(TEvent @event)
        where TEvent : IEvent => base.HandleAsync(@event);
}
