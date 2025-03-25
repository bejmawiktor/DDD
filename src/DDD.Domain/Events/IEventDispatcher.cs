using Utils.Disposable;

namespace DDD.Domain.Events;

public interface IEventDispatcher : IDispatcher<IEvent> { }
