namespace DDD.Domain.Events
{
    public interface IEventDispatcher
    {
        void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}