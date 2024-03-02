using DDD.Domain.Events;

namespace DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles
{
    public class EventStub : IEvent
    {
        public bool WasHandled { get; set; }
    }
}
