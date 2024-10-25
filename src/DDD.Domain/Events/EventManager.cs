using DDD.Domain.Common;

namespace DDD.Domain.Events;

public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    public override IDispatcher? Dispatcher { get; set; }

    public EventManager() { }
}