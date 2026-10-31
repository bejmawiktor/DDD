using DDD.Domain.Events;
using Utils.Persistence;

namespace DDD.Domain.Persistence;

/// <summary>
/// The synchronous counterpart of <see cref="IAsyncEventsTransactionScope"/>: an
/// <see cref="ITransactionScope"/> that can store the events of a
/// <see cref="TransactionalEventsScope"/> created for it. Events published from that scope
/// before the transaction commits are stored in it, committed with the data and delivered
/// only after the commit; publishing after the transaction completed or rolled back throws
/// <see cref="InvalidOperationException"/>.
/// </summary>
public interface IEventsTransactionScope : ITransactionScope
{
    /// <summary>
    /// Binds an events scope to this transaction by attaching the transaction's
    /// <see cref="ISyncTransactionalEventDispatcher"/> to it with
    /// <see cref="TransactionalEventsScope.AttachDispatcher(ISyncTransactionalEventDispatcher)"/>.
    /// Called by the <see cref="TransactionalEventsScope"/> constructor, not by the code
    /// raising the events.
    /// </summary>
    /// <param name="eventsScope">The scope whose events belong to the transaction.</param>
    void Attach(TransactionalEventsScope eventsScope);
}
