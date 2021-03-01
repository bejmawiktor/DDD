namespace DDD.Domain.Events
{
    public interface IEventSubscriber<TEvent>
        where TEvent : IEvent
    {
        void Handle(TEvent @event);
    }
}