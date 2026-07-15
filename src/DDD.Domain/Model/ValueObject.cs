namespace DDD.Domain.Model;

public abstract class ValueObject : IDomainObject
{
    public static bool operator ==(ValueObject? lhs, ValueObject? rhs) =>
        lhs is null ? rhs is null : lhs.Equals(rhs);

    public static bool operator !=(ValueObject? lhs, ValueObject? rhs) => !(lhs == rhs);

    protected abstract IEnumerable<object?> GetEqualityMembers();

    public override bool Equals(object? obj) =>
        obj is ValueObject other
        && this.GetType() == other.GetType()
        && this.GetEqualityMembers().SequenceEqual(other.GetEqualityMembers());

    public override int GetHashCode()
    {
        HashCode hash = new();
        hash.Add(this.GetType());

        foreach (object? memberValue in this.GetEqualityMembers())
        {
            hash.Add(memberValue);
        }

        return hash.ToHashCode();
    }
}

public abstract class ValueObject<TValue> : ValueObject
{
    protected TValue Value { get; }

    protected ValueObject(TValue value)
    {
        this.ValidateValue(value);

        this.Value = value;
    }

    protected abstract void ValidateValue(TValue value);

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Value;
    }

    public override string? ToString() => this.Value?.ToString();
}
