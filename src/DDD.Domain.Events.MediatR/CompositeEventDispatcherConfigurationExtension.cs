using MediatR;

namespace DDD.Domain.Events.MediatR;

public static class CompositeEventDispatcherConfigurationExtension
{
    public static CompositeEventDispatcherConfiguration WithMediatRDispatcher(
        this CompositeEventDispatcherConfiguration configuration,
        IMediator mediator
    )
    {
        ArgumentNullException.ThrowIfNull(mediator);

        return configuration.WithDispatcher(new EventDispatcher(mediator));
    }
}
