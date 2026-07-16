using MediatR;

namespace DDD.Domain.Events.MediatR;

public static class CompositeEventDispatcherConfigurationExtension
{
    extension(CompositeEventDispatcherConfiguration configuration)
    {
        public CompositeEventDispatcherConfiguration WithMediatRDispatcher(IMediator mediator)
        {
            ArgumentNullException.ThrowIfNull(mediator);

            return configuration.WithDispatcher(new EventDispatcher(mediator));
        }
    }
}
