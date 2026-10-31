using Utils.Disposable;

namespace DDD.Domain.Events;

/// <summary>
/// An asynchronous event dispatcher bound to an open transaction, receiving the events
/// of a <see cref="TransactionalEventsScope"/>. Dispatching an event must not deliver
/// it: the event has to be stored in that same transaction, so a rollback discards it
/// together with the data. Stored events are delivered only after the transaction
/// commits, and must still be delivered if the process stops between the commit and the
/// delivery. Delivery is at least once, so event handlers have to be idempotent.
/// </summary>
public interface IAsyncTransactionalEventDispatcher : IAsyncDispatcher<IEvent> { }
