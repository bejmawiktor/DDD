using Utils.Disposable;

namespace DDD.Domain.Events;

/// <summary>
/// A unit-of-work style scope that buffers the domain events raised through the
/// <see cref="EventManager"/> and releases them together when it is published.
/// This lets events be committed atomically alongside the work that produced
/// them. Discarding the scope without publishing drops the buffered events.
/// </summary>
public sealed class EventsScope : Scope<IEvent, EventsScope, EventManager>
{
    /// <summary>
    /// Publishes every event buffered in this scope synchronously.
    /// </summary>
    public void Publish() => base.Apply();

    /// <summary>
    /// Publishes every event buffered in this scope asynchronously.
    /// </summary>
    /// <returns>A task that completes once all buffered events have been dispatched.</returns>
    public Task PublishAsync() => base.ApplyAsync();
}
