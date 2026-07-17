namespace DDD.Domain.Events;

/// <summary>
/// Default <see cref="ICompositeEventDispatcher"/> implementation that forwards
/// each event to every registered inner dispatcher — synchronously in
/// declaration order, or asynchronously in parallel.
/// </summary>
public class CompositeEventDispatcher : ICompositeEventDispatcher
{
    /// <summary>
    /// Gets the inner dispatchers that receive every dispatched event.
    /// </summary>
    protected internal List<IEventDispatcher> Dispatchers { get; }

    /// <summary>
    /// Initializes a new, empty <see cref="CompositeEventDispatcher"/>.
    /// </summary>
    public CompositeEventDispatcher()
    {
        this.Dispatchers = [];
    }

    /// <summary>
    /// Adds a single inner event dispatcher.
    /// </summary>
    /// <param name="eventDispatcher">The event dispatcher to add.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="eventDispatcher"/> is <see langword="null"/>.
    /// </exception>
    public void Add(IEventDispatcher eventDispatcher)
    {
        ArgumentNullException.ThrowIfNull(eventDispatcher);

        this.Dispatchers.Add(eventDispatcher);
    }

    /// <summary>
    /// Adds several inner event dispatchers at once.
    /// </summary>
    /// <param name="eventDispatchers">The event dispatchers to add.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="eventDispatchers"/> is <see langword="null"/>.
    /// </exception>
    public void AddRange(IEnumerable<IEventDispatcher> eventDispatchers)
    {
        ArgumentNullException.ThrowIfNull(eventDispatchers);

        this.Dispatchers.AddRange(eventDispatchers);
    }

    /// <summary>
    /// Dispatches an event to every inner dispatcher, synchronously and in
    /// registration order.
    /// </summary>
    /// <typeparam name="TItem">The concrete event type.</typeparam>
    /// <param name="item">The event to dispatch.</param>
    public void Dispatch<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        this.Dispatchers.ForEach(dispatcher => dispatcher.Dispatch(item));

    /// <summary>
    /// Dispatches an event to every inner dispatcher asynchronously and in
    /// parallel.
    /// </summary>
    /// <typeparam name="TItem">The concrete event type.</typeparam>
    /// <param name="item">The event to dispatch.</param>
    /// <returns>A task that completes once every inner dispatcher has handled the event.</returns>
    public Task DispatchAsync<TItem>(TItem item)
        where TItem : notnull, IEvent =>
        Parallel.ForEachAsync(
            this.Dispatchers,
            async (dispatcher, token) => await dispatcher.DispatchAsync(item)
        );
}
