namespace DDD.Domain.Events;

/// <summary>
/// Marker interface for domain events — immutable records of something notable
/// that has happened in the domain. Implemented by every event so it can flow
/// through the dispatching infrastructure.
/// </summary>
public interface IEvent { }
