using MediatR;

namespace DDD.Domain.Events.MediatR
{
    public interface IEventHandler<TEvent> : INotificationHandler<Notification<TEvent>>
        where TEvent : IEvent
    {
        Task Handle(TEvent @event, CancellationToken cancellationToken);

        Task INotificationHandler<Notification<TEvent>>.Handle(
            Notification<TEvent> notification,
            CancellationToken cancellationToken
        )
        {
            return this.Handle(notification.Event, cancellationToken);
        }
    }
}
