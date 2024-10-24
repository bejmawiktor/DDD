using DDD.Domain.Common;
using System;
using System.Threading;

namespace DDD.Domain.Events;

public sealed class EventManager : IScopeHandler<EventsScope, IEvent, EventManager>
{
    static EventsScope? IScopeHandler<EventsScope, IEvent, EventManager>.CurrentScope
    {
        get => EventManager.CurrentScope;
        set => EventManager.CurrentScope = value;
    }

    static EventManager IScopeHandler<EventsScope, IEvent, EventManager>.Instance =>
        EventManager.Instance;

    private static readonly Lazy<EventManager> instance = new(() => new EventManager());

    private static readonly AsyncLocal<EventsScope?> localEventsScope = new();

    public static EventsScope? CurrentScope
    {
        get => EventManager.localEventsScope.Value;
        internal set => EventManager.localEventsScope.Value = value;
    }

    public static EventManager Instance => instance.Value;

    public IDispatcher? Dispatcher { get; set; }

    public EventManager() { }
}