namespace DDD.Domain.Model;

public abstract class Entity<TIdentifier> : IEntity<TIdentifier>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    public TIdentifier Id
    {
        get;
        protected set => field = value ?? throw new ArgumentNullException(nameof(value));
    }

    protected Entity(TIdentifier id)
    {
        ArgumentNullException.ThrowIfNull(id);

        this.Id = id;
    }

    public static bool operator ==(Entity<TIdentifier>? lhs, Entity<TIdentifier>? rhs) =>
        lhs is null ? rhs is null : lhs.Equals(rhs);

    public static bool operator !=(Entity<TIdentifier>? lhs, Entity<TIdentifier>? rhs) =>
        !(lhs == rhs);

    public override bool Equals(object? obj) =>
        obj is Entity<TIdentifier> other
        && this.GetType() == other.GetType()
        && this.Id.Equals(other.Id);

    public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Id);
}
