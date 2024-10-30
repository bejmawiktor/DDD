using MediatR;

namespace DDD.Domain.Events.MediatR;

public class EventNotification<TEvent> : INotification
    where TEvent : DDD.Domain.Events.IEvent
{
    public TEvent Event { get; }

    public EventNotification(TEvent @event)
    {
        this.Event = @event;
    }
}
