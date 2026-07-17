namespace DDD.Domain.Events;

/// <summary>
/// Base class for domain events. Assigns each event a unique <see cref="Id"/>
/// and a UTC <see cref="CreatedAt"/> timestamp at construction time.
/// </summary>
public abstract class Event : IEvent
{
    /// <summary>
    /// Gets the unique identifier assigned to this event instance.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets the UTC timestamp captured when the event was created.
    /// </summary>
    public DateTime CreatedAt { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Event"/> class, generating a
    /// new identifier and recording the current UTC time.
    /// </summary>
    protected Event()
    {
        this.Id = Guid.NewGuid();
        this.CreatedAt = DateTime.UtcNow;
    }
}
