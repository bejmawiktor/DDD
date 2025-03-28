﻿using System;

namespace DDD.Domain.Model;

public abstract class Entity<TIdentifier> : IEntity<TIdentifier>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    private TIdentifier id;

    public TIdentifier Id
    {
        get => this.id;
        protected set => this.id = value ?? throw new ArgumentNullException(nameof(this.id));
    }

    protected Entity(TIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        this.id = id;
    }

    public static bool operator ==(Entity<TIdentifier> lhs, Entity<TIdentifier> rhs) =>
        (lhs is null && rhs is null) || (lhs is not null && rhs is not null && lhs.Equals(rhs));

    public static bool operator !=(Entity<TIdentifier> lhs, Entity<TIdentifier> rhs) =>
        !(lhs == rhs);

    public override bool Equals(object? obj)
    {
        Entity<TIdentifier>? other = obj as Entity<TIdentifier>;

        return this.GetType().Equals(other?.GetType()) && this.Id.Equals(other!.Id);
    }

    public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Id);
}
