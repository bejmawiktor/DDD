using MediatR;

namespace DDD.Domain.Events.MediatR;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent, INotification { }
