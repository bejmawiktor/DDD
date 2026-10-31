using DDD.Domain.Events;
using DDD.Domain.Persistence;
using Utils.Persistence;

namespace DDD.Application.Persistence;

/// <summary>
/// An <see cref="IAsyncUnitOfWork"/> whose transactions can store the events of a
/// <see cref="TransactionalEventsScope"/> in the same transaction as the data.
/// </summary>
public interface IAsyncEventsUnitOfWork : IAsyncUnitOfWork
{
    /// <summary>
    /// Asynchronously begins a new transaction able to store events. Create a
    /// <see cref="TransactionalEventsScope"/> for it to collect the events of the
    /// operation; whether they are stored is the caller's decision. Those published with
    /// <see cref="TransactionalEventsScope.PublishAsync"/> before
    /// <see cref="IAsyncTransactionScope.CompleteAsync"/> are committed with the data and
    /// delivered only after the commit; unpublished ones are dropped. Disposing the
    /// returned scope without completing it rolls back both the data and the published
    /// events.
    /// </summary>
    /// <returns>A task producing the transaction scope controlling commit and rollback.</returns>
    new Task<IAsyncEventsTransactionScope> BeginTransactionAsync();

    async Task<IAsyncTransactionScope> IAsyncUnitOfWork.BeginTransactionAsync() =>
        await this.BeginTransactionAsync();
}
