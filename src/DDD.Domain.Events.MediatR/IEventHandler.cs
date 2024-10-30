using MediatR;

namespace DDD.Domain.Events.MediatR;

public interface IEventHandler<TEvent> : INotificationHandler<EventNotification<TEvent>>
    where TEvent : IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);

    Task INotificationHandler<EventNotification<TEvent>>.Handle(
        DDD.Domain.Events.MediatR.EventNotification<TEvent> notification,
        System.Threading.CancellationToken cancellationToken
    ) => this.Handle(notification.Event, cancellationToken);
}
