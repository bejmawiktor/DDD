namespace DDD.Domain.Events;

public interface ICompositeEventDispatcher : IEventDispatcher
{
    void Add(IEventDispatcher eventDispatcher);
}
