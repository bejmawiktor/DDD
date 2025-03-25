using System.Buffers.Text;
using System.Threading.Tasks;
using Utils.Disposable;

namespace DDD.Domain.Events;

public sealed class EventManager : ScopeHandler<EventsScope, IEvent, EventManager>
{
    public override IDispatcher<IEvent>? Dispatcher { get; set; }

    public EventManager() { }

    public void Notify<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent => base.Handle(@event);

    public Task NotifyAsync<TEvent>(TEvent @event)
        where TEvent : notnull, IEvent => base.HandleAsync(@event);
}
