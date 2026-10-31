using DDD.Domain.Events;
using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class AsyncEventsTransactionScopeFake(IAsyncTransactionalEventDispatcher? dispatcher = null)
    : IAsyncEventsTransactionScope
{
    public TransactionalEventsScope? AttachedEventsScope { get; private set; }

    public void Attach(TransactionalEventsScope eventsScope)
    {
        this.AttachedEventsScope = eventsScope;

        if (dispatcher is not null)
        {
            eventsScope.AttachDispatcher(dispatcher);
        }
    }

    public Task CompleteAsync() => Task.CompletedTask;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}
