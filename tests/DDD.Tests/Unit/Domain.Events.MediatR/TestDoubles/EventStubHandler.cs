using DDD.Domain.Events.MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Domain.Events.MediatR.TestDoubles
{
    public class EventStubHandler : IEventHandler<EventStub>
    {
        public Task Handle(EventStub @event, CancellationToken cancellationToken)
        {
            return Task.Run(() => @event.WasHandled = true);
        }
    }
}