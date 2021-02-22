using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace DDD.Events
{
    public sealed class EventManager
    {
        private static readonly Lazy<EventManager> instance = new Lazy<EventManager>(
            () => new EventManager());

        private static readonly AsyncLocal<EventsScope> LocalEventsScope = new AsyncLocal<EventsScope>();

        public static EventsScope CurrentScope
        {
            get => LocalEventsScope.Value;
            internal set => LocalEventsScope.Value = value;
        }

        public static EventManager Instance => instance.Value;

        public IEventDispatcher EventDispatcher { get; set; }

        private EventManager()
        {
        }

        public EventsScope CreateScope()
        {
            if(EventManager.CurrentScope != null)
            {
                throw new InvalidOperationException("Can't begin another scope when last one wasn't disposed.");
            }

            return EventManager.CurrentScope = new EventsScope();
        }

        public void Notify<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(EventManager.CurrentScope == null)
            {
                this.EventDispatcher?.Dispatch(@event);
            }
            else
            {
                EventManager.CurrentScope.AddEvent(@event);
            }
        }
    }
}