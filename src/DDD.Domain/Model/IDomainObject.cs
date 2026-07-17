namespace DDD.Domain.Model;

/// <summary>
/// Marker interface implemented by every domain building block — entities,
/// value objects and aggregate roots. It declares no members and exists only
/// to constrain generic types to objects that belong to the domain model.
/// </summary>
public interface IDomainObject { }
