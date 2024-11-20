using System.Threading.Tasks;
using DDD.Domain.Utils;

namespace DDD.Domain.Events;

public sealed class EventsScope : Scope<IEvent, EventsScope, EventManager>
{
    public void Publish() => base.Apply();

    public Task PublishAsync() => base.ApplyAsync();
}
