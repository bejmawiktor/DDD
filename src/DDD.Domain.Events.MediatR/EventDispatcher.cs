using MediatR;

namespace DDD.Domain.Events.MediatR
{
    internal class EventDispatcher : IEventDispatcher
    {
        private IMediator Mediator { get; }

        public EventDispatcher(IMediator mediator)
        {
            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            this.Mediator.Publish(new Notification<TEvent>(@event)).GetAwaiter().GetResult();
        }

        public Task DispatchAsync<TEvent>(TEvent @event)
            where TEvent : IEvent
        {
            return this.Mediator.Publish(new Notification<TEvent>(@event));
        }
    }
}