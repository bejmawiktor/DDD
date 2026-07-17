using Utils.Disposable;

namespace DDD.Domain.Events;

/// <summary>
/// Dispatches domain events to their handlers. Specializes the generic
/// <see cref="IDispatcher{T}"/> from the Utils library to
/// <see cref="IEvent"/>, so implementations decide how events are delivered
/// (for example in-process, through MediatR or via a message bus).
/// </summary>
public interface IEventDispatcher : IDispatcher<IEvent> { }
