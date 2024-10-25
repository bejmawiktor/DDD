using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public sealed class EventsScope : Scope<IEvent, EventsScope, EventManager> { }
