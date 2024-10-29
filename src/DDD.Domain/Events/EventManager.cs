using System.Threading.Tasks;
using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    public override IDispatcher<IEvent>? Dispatcher { get; set; }

    public EventManager() { }

    public void Notify(IEvent @event) => base.Handle(@event);

    public Task NotifyAsync(IEvent @event) => base.HandleAsync(@event);
}
