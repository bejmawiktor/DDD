using DDD.Domain.Events;
using Utils.Persistence;

namespace DDD.Application.Persistence;

/// <summary>
/// An <see cref="IAsyncUnitOfWork"/> whose transactions can take ownership of a
/// <see cref="TransactionalEventsScope"/>, so the events raised by an operation are
/// stored in the same transaction as its data.
/// </summary>
public interface IAsyncEventsUnitOfWork : IAsyncUnitOfWork
{
    /// <summary>
    /// Asynchronously begins a new transaction that owns the events buffered in
    /// <paramref name="eventsScope"/>. Completing the returned scope publishes those
    /// events to the transaction's <see cref="IAsyncTransactionalEventDispatcher"/>, commits,
    /// and only then delivers them. Disposing it without calling
    /// <see cref="IAsyncTransactionScope.CompleteAsync"/> rolls back both the data and the
    /// events.
    /// </summary>
    /// <param name="eventsScope">The scope whose events belong to the transaction.</param>
    /// <returns>A task producing the transaction scope controlling commit and rollback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="eventsScope"/> is <see langword="null"/>.</exception>
    Task<IAsyncTransactionScope> BeginTransactionAsync(TransactionalEventsScope eventsScope);
}
