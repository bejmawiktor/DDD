namespace DDD.Domain.Events;

/// <summary>
/// Fluent builder used to declare which inner dispatchers a
/// <see cref="CompositeEventDispatcher"/> should forward events to.
/// </summary>
public class CompositeEventDispatcherConfiguration
{
    /// <summary>
    /// Gets the dispatchers collected so far.
    /// </summary>
    protected internal List<IEventDispatcher> Dispatchers { get; }

    /// <summary>
    /// Initializes a new, empty <see cref="CompositeEventDispatcherConfiguration"/>.
    /// </summary>
    public CompositeEventDispatcherConfiguration()
    {
        this.Dispatchers = [];
    }

    /// <summary>
    /// Registers an inner dispatcher and returns the same configuration to allow
    /// chaining.
    /// </summary>
    /// <param name="dispatcher">The dispatcher to register.</param>
    /// <returns>This configuration instance, for fluent chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="dispatcher"/> is <see langword="null"/>.
    /// </exception>
    public CompositeEventDispatcherConfiguration WithDispatcher(IEventDispatcher dispatcher)
    {
        ArgumentNullException.ThrowIfNull(dispatcher);

        this.Dispatchers.Add(dispatcher);

        return this;
    }
}
