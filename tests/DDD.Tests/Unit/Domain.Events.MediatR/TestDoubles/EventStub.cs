using DDD.Domain.Events;
using DDD.Domain.Events.MediatR;

namespace DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;

public class EventStub : DDD.Domain.Events.IEvent
{
    public bool WasHandled { get; set; }
}
