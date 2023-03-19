using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Domain.Events
{
    public sealed class EventManager
    {
        private static readonly Lazy<EventManager> instance = new Lazy<EventManager>(
            () => new EventManager());

        private static readonly AsyncLocal<EventsScope?> localEventsScope = new AsyncLocal<EventsScope?>();

        public static EventsScope? CurrentScope
        {
            get => EventManager.localEventsScope.Value;
            internal set => EventManager.localEventsScope.Value = value;
        }

        public static EventManager Instance => instance.Value;

        public IEventDispatcher? EventDispatcher { get; set; }

        private EventManager()
        {
        }

        public void Notify<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(EventManager.CurrentScope is null)
            {
                this.EventDispatcher?.Dispatch(@event);
            }
            else
            {
                EventManager.CurrentScope.AddEvent(@event);
            }
        }

        public Task NotifyAsync<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if(EventManager.CurrentScope == null)
            {
                return this.EventDispatcher?.DispatchAsync(@event) ?? Task.CompletedTask;
            }

            EventManager.CurrentScope.AddEvent(@event);

            return Task.CompletedTask;
        }
    }
}