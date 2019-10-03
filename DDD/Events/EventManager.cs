using System;
using System.Diagnostics.CodeAnalysis;

namespace DDD.Events
{
    [ExcludeFromCodeCoverage]
    public sealed class EventManager
    {
        private static readonly Lazy<EventManager> instance = new Lazy<EventManager>(
            () => new EventManager());

        public IEventDispatcher EventDispatcher { get; set; }

        public static EventManager Instance => instance.Value;

        private EventManager()
        {
        }

        public void Notify<TEvent>(TEvent @event) where TEvent : IEvent
        {
            this.EventDispatcher?.Dispatch(@event);
        }
    }
}