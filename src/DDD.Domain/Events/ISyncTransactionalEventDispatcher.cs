using Utils.Disposable;

namespace DDD.Domain.Events;

/// <summary>
/// The synchronous counterpart of <see cref="IAsyncTransactionalEventDispatcher"/>: an
/// event dispatcher bound to an open transaction, receiving the events of a
/// <see cref="TransactionalEventsScope"/> it was attached to with
/// <see cref="TransactionalEventsScope.AttachDispatcher(ISyncTransactionalEventDispatcher)"/>
/// by the transaction that owns it. Dispatching after the transaction
/// completed must throw <see cref="InvalidOperationException"/>. Dispatching an event
/// must not deliver it: the event has to be stored in that same
/// transaction, so a rollback discards it together with the data. Stored events are
/// delivered only after the transaction commits, and must still be delivered if the
/// process stops between the commit and the delivery. Delivery is at least once, so
/// event handlers have to be idempotent.
/// </summary>
/// <remarks>
/// Implement it only over a store that writes synchronously; blocking on an
/// asynchronous store from here is what the asynchronous counterpart exists to avoid.
/// </remarks>
public interface ISyncTransactionalEventDispatcher : ISyncDispatcher<IEvent> { }
