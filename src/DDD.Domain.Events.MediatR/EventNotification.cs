using MediatR;

namespace DDD.Domain.Events.MediatR;

public class EventNotification<TEvent>(TEvent @event) : INotification
    where TEvent : notnull, DDD.Domain.Events.IEvent
{
    public TEvent Event { get; } = @event;
}
