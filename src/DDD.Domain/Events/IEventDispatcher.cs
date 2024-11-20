using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public interface IEventDispatcher : IDispatcher<IEvent> { }
