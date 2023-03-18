using MediatR;

namespace DDD.Domain.Events.MediatR
{
    public class Notification<TEvent> : INotification
        where TEvent : IEvent
    {
        public TEvent Event { get; }

        public Notification(TEvent @event)
        {
            this.Event = @event;
        }
    }
}