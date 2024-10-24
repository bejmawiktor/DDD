using DDD.Domain.Events.MediatR;

namespace DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;

public class EventStub : IEventNotification
{
    public bool WasHandled { get; set; }
}