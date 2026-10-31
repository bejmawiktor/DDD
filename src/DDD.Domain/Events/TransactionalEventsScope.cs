using DDD.Domain.Persistence;

namespace DDD.Domain.Events;

/// <summary>
/// An events scope whose events belong to a transaction. Like <see cref="EventsScope"/>
/// it buffers the events raised through the <see cref="EventManager"/>, but it releases
/// them only to the dispatcher of the transaction it was created for, never to the
/// <see cref="EventManager"/>'s global dispatcher. Create it for an open transaction;
/// then decide whether to publish: <see cref="PublishAsync"/> or <see cref="Publish"/>
/// before the transaction completes stores the events atomically with the data, while a
/// scope disposed without publishing drops them. The scope neither begins nor commits a
/// transaction itself.
/// </summary>
/// <remarks>
/// It must be the outermost events scope: a nested scope forwards its events to the
/// parent, which would take them out of the transaction. Create it with <c>new</c> in
/// the method that raises the events, not inside an <c>async</c> method it calls, since
/// the ambient scope set there does not flow back to the caller.
/// </remarks>
public sealed class TransactionalEventsScope : IDisposable
{
    private EventsScope EventsScope { get; }
    private IAsyncTransactionalEventDispatcher? AsyncDispatcher { get; set; }
    private ISyncTransactionalEventDispatcher? SyncDispatcher { get; set; }

    /// <summary>
    /// Initializes a transactional events scope for an asynchronous transaction, attaches
    /// it to the transaction and makes it the ambient events scope.
    /// </summary>
    /// <param name="transactionScope">The open transaction the events belong to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="transactionScope"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Another events scope is already active.</exception>
    public TransactionalEventsScope(IAsyncEventsTransactionScope transactionScope)
    {
        ArgumentNullException.ThrowIfNull(transactionScope);

        TransactionalEventsScope.ThrowIfNotOutermost();

        transactionScope.Attach(this);

        this.EventsScope = new EventsScope();
    }

    /// <summary>
    /// Initializes a transactional events scope for a synchronous transaction, attaches it
    /// to the transaction and makes it the ambient events scope.
    /// </summary>
    /// <param name="transactionScope">The open transaction the events belong to.</param>
    /// <exception cref="ArgumentNullException"><paramref name="transactionScope"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Another events scope is already active.</exception>
    public TransactionalEventsScope(IEventsTransactionScope transactionScope)
    {
        ArgumentNullException.ThrowIfNull(transactionScope);

        TransactionalEventsScope.ThrowIfNotOutermost();

        transactionScope.Attach(this);

        this.EventsScope = new EventsScope();
    }

    /// <summary>
    /// Attaches the scope to an asynchronous transaction. Called by the transaction's
    /// <see cref="IAsyncEventsTransactionScope.Attach"/>, not by the code raising the events.
    /// </summary>
    /// <param name="dispatcher">The dispatcher storing events in the transaction.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The scope is already attached to a transaction.</exception>
    public void AttachDispatcher(IAsyncTransactionalEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.ThrowIfAttached();

        this.AsyncDispatcher = dispatcher;
    }

    /// <summary>
    /// Attaches the scope to a synchronous transaction. Called by the transaction's
    /// <see cref="IEventsTransactionScope.Attach"/>, not by the code raising the events.
    /// </summary>
    /// <param name="dispatcher">The dispatcher storing events in the transaction.</param>
    /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The scope is already attached to a transaction.</exception>
    public void AttachDispatcher(ISyncTransactionalEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.ThrowIfAttached();

        this.SyncDispatcher = dispatcher;
    }

    /// <summary>
    /// Publishes every buffered event synchronously into the attached transaction.
    /// </summary>
    /// <exception cref="InvalidOperationException">The scope is not attached to a synchronous transaction.</exception>
    public void Publish()
    {
        if (this.SyncDispatcher is null)
        {
            throw new InvalidOperationException(
                "Transactional events scope is not attached to a synchronous transaction."
            );
        }

        this.EventsScope.Publish(this.SyncDispatcher);
    }

    /// <summary>
    /// Publishes every buffered event asynchronously into the attached transaction.
    /// </summary>
    /// <returns>A task that completes once all buffered events have been dispatched.</returns>
    /// <exception cref="InvalidOperationException">The scope is not attached to an asynchronous transaction.</exception>
    public Task PublishAsync()
    {
        if (this.AsyncDispatcher is null)
        {
            throw new InvalidOperationException(
                "Transactional events scope is not attached to an asynchronous transaction."
            );
        }

        return this.EventsScope.PublishAsync(this.AsyncDispatcher);
    }

    /// <summary>
    /// Disposes the scope, dropping any events that were not published.
    /// </summary>
    public void Dispose() => this.EventsScope.Dispose();

    private static void ThrowIfNotOutermost()
    {
        if (EventManager.CurrentScope is not null)
        {
            throw new InvalidOperationException(
                "Transactional events scope must be the outermost events scope."
            );
        }
    }

    private void ThrowIfAttached()
    {
        if (this.AsyncDispatcher is not null || this.SyncDispatcher is not null)
        {
            throw new InvalidOperationException(
                "Transactional events scope is already attached to a transaction."
            );
        }
    }
}
