using MediatR;

namespace DDD.Domain.Events.MediatR
{
    public class EventDispatcher : IEventDispatcher
    {
        private IMediator Mediator { get; }

        public EventDispatcher(IMediator mediator)
        {
            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            this.Mediator.Publish(new Notification<TEvent>(@event));
        }
    }
}