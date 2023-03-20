using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Domain.Events
{
    public class EventsScope : IDisposable
    {
        private bool IsDisposed { get; set; }
        internal List<IEvent> Events { get; }
        private EventsScope? ParentScope { get; }
        private int NestedScopesCounter { get; set; }

        public EventsScope()
        {
            this.Events = new List<IEvent>();
            this.ParentScope = EventManager.CurrentScope;

            if(this.ParentScope is not null)
            {
                this.ParentScope.NestedScopesCounter++;
            }

            EventManager.CurrentScope = this;
        }

        internal void Add<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            ArgumentNullException.ThrowIfNull(@event);

            this.Events.Add(@event);
        }

        public void Publish()
        {
            if(this.ParentScope is null)
            {
                foreach(IEvent @event in this.Events)
                {
                    EventManager.Instance.EventDispatcher?.Dispatch((dynamic)@event);
                }
            }
            else
            {
                this.ParentScope.AddRange(this.Events);
            }

            this.Clear();
        }

        public Task PublishAsync()
        {
            List<Task> tasks = new();

            if(this.ParentScope is null)
            {
                foreach(IEvent @event in this.Events)
                {
                    if(EventManager.Instance.EventDispatcher is not null)
                    {
                        tasks.Add(EventManager.Instance.EventDispatcher?.DispatchAsync((dynamic)@event));
                    }
                }
            }
            else
            {
                this.ParentScope.AddRange(this.Events);
            }

            this.Clear();

            return Task.WhenAll(tasks);
        }

        private void AddRange(IEnumerable<IEvent> events)
        {
            this.Events.AddRange(events);
        }

        public void Clear()
        {
            this.Events.Clear();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if(!this.IsDisposed)
            {
                if(this.ParentScope is not null)
                {
                    this.ParentScope.NestedScopesCounter--;
                }

                if(this.NestedScopesCounter > 0)
                {
                    this.Clear();
                    throw new InvalidOperationException("EventsScope nested incorrectly.");
                }

                if(disposing)
                {
                    this.Clear();
                }

                if(ReferenceEquals(this, EventManager.CurrentScope))
                {
                    EventManager.CurrentScope = this.ParentScope;
                }
            }

            this.IsDisposed = true;
        }

        ~EventsScope()
        {
            this.Dispose(false);
        }
    }
}