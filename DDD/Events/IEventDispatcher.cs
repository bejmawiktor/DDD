namespace DDD.Events
{
    public interface IEventDispatcher
    {
        void Dispatch<TEvent>(TEvent @event)
            where TEvent : IEvent;

        void Dispatch(IEvent @event);
    }
}