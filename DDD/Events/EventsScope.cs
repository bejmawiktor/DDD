using System;
using System.Collections.Generic;

namespace DDD.Events
{
    public class EventsScope : IDisposable
    {
        private bool IsDisposed { get; set; }
        internal List<IEvent> Events { get; }

        internal EventsScope()
        {
            this.Events = new List<IEvent>();
        }

        internal void AddEvent<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            if(@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }

            this.Events.Add(@event);
        }

        public void Publish()
        {
            foreach(var @event in this.Events)
            {
                EventManager.Instance.EventDispatcher.Dispatch(@event);
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
                if(disposing)
                {
                    this.Clear();
                }

                if(ReferenceEquals(this, EventManager.CurrentScope))
                {
                    EventManager.CurrentScope = null;
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