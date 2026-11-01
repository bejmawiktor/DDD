using DDD.Domain.Events;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class RecordingTransactionalEventDispatcherFake : IAsyncTransactionalEventDispatcher
{
    private readonly List<IEvent> dispatchedEvents = [];
    private int inProgressCount;

    public IReadOnlyCollection<IEvent> DispatchedEvents => this.dispatchedEvents.AsReadOnly();
    public int MaxInProgressCount { get; private set; }

    public async Task DispatchAsync<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent
    {
        int inProgressCount = Interlocked.Increment(ref this.inProgressCount);
        this.MaxInProgressCount = Math.Max(this.MaxInProgressCount, inProgressCount);

        await Task.Yield();

        _ = Interlocked.Decrement(ref this.inProgressCount);

        this.dispatchedEvents.Add(@event);
    }
}
