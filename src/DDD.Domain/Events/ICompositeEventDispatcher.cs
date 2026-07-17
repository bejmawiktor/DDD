namespace DDD.Domain.Events;

/// <summary>
/// An <see cref="IEventDispatcher"/> that fans every event out to a set of
/// inner dispatchers, allowing several delivery mechanisms to be combined
/// behind a single dispatcher.
/// </summary>
public interface ICompositeEventDispatcher : IEventDispatcher
{
    /// <summary>
    /// Adds an inner dispatcher that will subsequently receive every dispatched
    /// event.
    /// </summary>
    /// <param name="eventDispatcher">The dispatcher to add to the composite.</param>
    void Add(IEventDispatcher eventDispatcher);
}
