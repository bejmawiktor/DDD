using DDD.Domain.Events;
using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class EventsTransactionScopeFake(ISyncTransactionalEventDispatcher? dispatcher = null)
    : IEventsTransactionScope
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

    public void Complete() { }

    public void Dispose() { }
}
