using System;
using System.Collections;
using System.Collections.Generic;

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

            if(this.ParentScope != null)
            {
                this.ParentScope.NestedScopesCounter++;
            }

            EventManager.CurrentScope = this;
        }

        internal void AddEvent<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            ArgumentNullException.ThrowIfNull(@event);

            this.Events.Add(@event);
        }

        public void Publish()
        {
            foreach(IEvent @event in this.Events)
            {
                if(this.ParentScope == null)
                {
                    EventManager.Instance.EventDispatcher?.Dispatch((dynamic)@event);
                }
                else
                {
                    this.ParentScope.AddEvent((dynamic)@event);
                }
            }

            this.Clear();
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
                if(this.ParentScope != null)
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