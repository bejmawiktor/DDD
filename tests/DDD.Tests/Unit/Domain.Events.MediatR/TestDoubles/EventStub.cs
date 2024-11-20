namespace DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles;

public class EventStub : DDD.Domain.Events.IEvent
{
    public bool WasHandled { get; set; }
}
