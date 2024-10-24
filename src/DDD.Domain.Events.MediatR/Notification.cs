using MediatR;

namespace DDD.Domain.Events.MediatR;

public interface IEventNotification : INotification, IEvent
{
}