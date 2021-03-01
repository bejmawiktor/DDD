using System;

namespace DDD.Domain.Events
{
    public abstract class Event : IEvent
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        public Event()
        {
            this.Id = Guid.NewGuid();
            this.CreatedAt = DateTime.Now;
        }
    }
}