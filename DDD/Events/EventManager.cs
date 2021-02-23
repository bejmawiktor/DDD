using System;
using System.Threading;

namespace DDD.Events
{
    public sealed class EventManager
    {
        private static readonly Lazy<EventManager> instance = new Lazy<EventManager>(
            () => new EventManager());

        private static readonly AsyncLocal<EventsScope> localEventsScope = new AsyncLocal<EventsScope>();

        public static EventsScope CurrentScope
        {
            get => EventManager.localEventsScope.Value;
            internal set => EventManager.localEventsScope.Value = value;
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