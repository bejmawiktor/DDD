using MediatR;

namespace DDD.Domain.Events.MediatR;

/// <summary>
/// Extension methods that register a MediatR dispatcher within a
/// <see cref="CompositeEventDispatcherConfiguration"/>.
/// </summary>
public static class CompositeEventDispatcherConfigurationExtension
{
    /// <summary>
    /// Adds a MediatR-based dispatcher to the composite configuration.
    /// </summary>
    /// <param name="configuration">The composite configuration to extend.</param>
    /// <param name="mediator">The MediatR mediator used to publish events.</param>
    /// <returns>The same configuration instance, for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="mediator"/> is <see langword="null"/>.
    /// </exception>
    public static CompositeEventDispatcherConfiguration WithMediatRDispatcher(
        this CompositeEventDispatcherConfiguration configuration,
        IMediator mediator
    )
    {
        ArgumentNullException.ThrowIfNull(mediator);

        return configuration.WithDispatcher(new EventDispatcher(mediator));
    }
}
