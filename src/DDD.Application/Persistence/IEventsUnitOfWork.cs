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
    /// Begins a new transaction that owns the events buffered in
    /// <paramref name="eventsScope"/>. The transaction publishes those events to its
    /// <see cref="ISyncTransactionalEventDispatcher"/> before it commits, and delivers
    /// them only after it commits. Disposing the returned scope without calling
    /// <see cref="ITransactionScope.Complete"/> rolls back both the data and the events.
    /// </summary>
    /// <param name="eventsScope">The scope whose events belong to the transaction.</param>
    /// <returns>The transaction scope controlling commit and rollback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="eventsScope"/> is <see langword="null"/>.</exception>
    ITransactionScope BeginTransaction(TransactionalEventsScope eventsScope);
}
