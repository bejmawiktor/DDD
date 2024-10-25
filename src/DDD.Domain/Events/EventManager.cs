using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    public override IDispatcher<IEvent>? Dispatcher { get; set; }

    public EventManager() { }
}
