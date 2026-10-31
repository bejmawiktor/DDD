using DDD.Domain.Events;
using DDD.Domain.Persistence;
using Utils.Persistence;

namespace DDD.Application.Persistence;

/// <summary>
/// The synchronous counterpart of <see cref="IAsyncEventsUnitOfWork"/>: an
/// <see cref="IUnitOfWork"/> whose transactions can store the events of a
/// <see cref="TransactionalEventsScope"/> in the same transaction as the data.
/// </summary>
public interface IEventsUnitOfWork : IUnitOfWork
{
    /// <summary>
    /// Begins a new transaction able to store events. Create a
    /// <see cref="TransactionalEventsScope"/> for it to collect the events of the
    /// operation; whether they are stored is the caller's decision. Those published with
    /// <see cref="TransactionalEventsScope.Publish"/> before the transaction commits are
    /// committed with the data and delivered only after the commit; unpublished ones are
    /// dropped. Disposing the returned scope without calling
    /// <see cref="ITransactionScope.Complete"/> rolls back both the data and the published
    /// events.
    /// </summary>
    /// <returns>The transaction scope controlling commit and rollback.</returns>
    new IEventsTransactionScope BeginTransaction();

    ITransactionScope IUnitOfWork.BeginTransaction() => this.BeginTransaction();
}
