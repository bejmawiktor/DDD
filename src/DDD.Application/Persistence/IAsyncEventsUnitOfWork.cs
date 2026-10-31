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
    /// Asynchronously begins a new transaction and attaches its
    /// <see cref="IAsyncTransactionalEventDispatcher"/> to <paramref name="eventsScope"/>
    /// with <see cref="TransactionalEventsScope.AttachDispatcher(IAsyncTransactionalEventDispatcher)"/>.
    /// Whether the events are stored is the caller's decision: those published with
    /// <see cref="TransactionalEventsScope.PublishAsync"/> before
    /// <see cref="IAsyncTransactionScope.CompleteAsync"/> are stored in the transaction,
    /// committed with the data and delivered only after the commit; unpublished ones are
    /// dropped. Publishing after the transaction completed throws
    /// <see cref="InvalidOperationException"/>. Disposing the returned scope without
    /// completing it rolls back both the data and the published events.
    /// </summary>
    /// <param name="eventsScope">The scope whose events belong to the transaction.</param>
    /// <returns>A task producing the transaction scope controlling commit and rollback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="eventsScope"/> is <see langword="null"/>.</exception>
    Task<IAsyncTransactionScope> BeginTransactionAsync(TransactionalEventsScope eventsScope);
}
