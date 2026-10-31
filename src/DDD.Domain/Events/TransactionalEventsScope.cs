namespace DDD.Domain.Events;

/// <summary>
/// An events scope whose events belong to a transaction. Like <see cref="EventsScope"/>
/// it buffers the events raised through the <see cref="EventManager"/>, but it releases
/// them only to an <see cref="ITransactionalEventDispatcher"/>, never to the
/// <see cref="EventManager"/>'s global dispatcher. Hand it to the unit of work that
/// begins the transaction; the transaction publishes the events before it commits, so
/// they are stored atomically with the data. The scope neither begins nor commits a
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

    /// <summary>
    /// Initializes a new transactional events scope and makes it the ambient events
    /// scope.
    /// </summary>
    /// <exception cref="InvalidOperationException">Another events scope is already active.</exception>
    public TransactionalEventsScope()
    {
        if (EventManager.CurrentScope is not null)
        {
            throw new InvalidOperationException(
                "Transactional events scope must be the outermost events scope."
            );
        }

        this.EventsScope = new EventsScope();
    }

    /// <summary>
    /// Publishes every buffered event to the transaction's dispatcher.
    /// </summary>
    /// <param name="dispatcher">The dispatcher storing the events in the transaction.</param>
    /// <returns>A task that completes once all buffered events have been dispatched.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dispatcher"/> is <see langword="null"/>.</exception>
    public Task PublishAsync(ITransactionalEventDispatcher dispatcher) =>
        this.EventsScope.PublishAsync(dispatcher);

    /// <summary>
    /// Disposes the scope, dropping any events that were not published.
    /// </summary>
    public void Dispose() => this.EventsScope.Dispose();
}
