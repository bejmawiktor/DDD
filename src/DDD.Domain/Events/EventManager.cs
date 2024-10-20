﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Events;

public sealed class EventManager
{
    private static readonly Lazy<EventManager> instance = new(() => new EventManager());

    private static readonly AsyncLocal<EventsScope?> localEventsScope = new();

    public static EventsScope? CurrentScope
    {
        get => EventManager.localEventsScope.Value;
        internal set => EventManager.localEventsScope.Value = value;
    }

    public static EventManager Instance => instance.Value;

    public IEventDispatcher? EventDispatcher { get; set; }

    private EventManager() { }

    public void Notify<TEvent>(TEvent @event)
        where TEvent : IEvent
    {
        if (EventManager.CurrentScope is null)
        {
            if (this.EventDispatcher is null)
            {
                throw new InvalidOperationException("Event dispatcher is uninitialized.");
            }

            this.EventDispatcher.Dispatch(@event);
        }
        else
        {
            EventManager.CurrentScope.Add(@event);
        }
    }

    public Task NotifyAsync<TEvent>(TEvent @event)
        where TEvent : IEvent
    {
        if (EventManager.CurrentScope is null)
        {
            return this.EventDispatcher is null
                ? throw new InvalidOperationException("Event dispatcher is uninitialized.")
                : this.EventDispatcher.DispatchAsync(@event);
        }

        EventManager.CurrentScope.Add(@event);

        return Task.CompletedTask;
    }
}
