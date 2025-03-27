using System;

namespace DDD.Domain.Model;

public abstract class AggregateRoot<TIdentifier>(TIdentifier id)
    : Entity<TIdentifier>(id),
        IAggregateRoot<TIdentifier>
    where TIdentifier : notnull, IEquatable<TIdentifier> { }
