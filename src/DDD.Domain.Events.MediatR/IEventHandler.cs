using MediatR;

namespace DDD.Domain.Events.MediatR;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEventNotification
{
}