using DDD.Domain.Events;
using Utils.Persistence;

namespace DDD.Application.Persistence;

/// <summary>
/// The synchronous counterpart of <see cref="IAsyncEventsUnitOfWork"/>: an
/// <see cref="IUnitOfWork"/> whose transactions can take ownership of a
/// <see cref="TransactionalEventsScope"/>, so the events raised by an operation are
/// stored in the same transaction as its data.
/// </summary>
public interface IEventsUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Begins a new transaction and attaches its
    /// <see cref="ISyncTransactionalEventDispatcher"/> to <paramref name="eventsScope"/>
    /// with <see cref="TransactionalEventsScope.AttachDispatcher(ISyncTransactionalEventDispatcher)"/>.
    /// Whether the events are stored is the caller's decision: those published with
    /// <see cref="TransactionalEventsScope.Publish"/> before the transaction commits are
    /// stored in it, committed with the data and delivered only after the commit;
    /// unpublished ones are dropped. Publishing after the transaction completed throws
    /// <see cref="InvalidOperationException"/>. Disposing the returned scope without
    /// calling <see cref="ITransactionScope.Complete"/> rolls back both the data and the
    /// published events.
    /// </summary>
    /// <param name="eventsScope">The scope whose events belong to the transaction.</param>
    /// <returns>The transaction scope controlling commit and rollback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="eventsScope"/> is <see langword="null"/>.</exception>
    ITransactionScope BeginTransaction(TransactionalEventsScope eventsScope);
}
