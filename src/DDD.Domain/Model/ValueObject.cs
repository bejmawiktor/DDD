namespace DDD.Domain.Model;

/// <summary>
/// Base class for value objects: immutable domain concepts that have no
/// identity and are compared by the values of their components. Equality is
/// structural — two value objects of the same type are equal when the members
/// returned by <see cref="GetEqualityMembers"/> are equal in sequence.
/// </summary>
public abstract class ValueObject : IDomainObject, IEquatable<ValueObject>
{
    /// <summary>
    /// Determines whether two value objects are structurally equal.
    /// </summary>
    /// <param name="lhs">The left-hand value object, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand value object, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if both are <see langword="null"/> or structurally
    /// equal; otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(ValueObject? lhs, ValueObject? rhs) =>
        lhs is null ? rhs is null : lhs.Equals(rhs);

    /// <summary>
    /// Determines whether two value objects are structurally different.
    /// </summary>
    /// <param name="lhs">The left-hand value object, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand value object, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value objects differ; otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(ValueObject? lhs, ValueObject? rhs) => !(lhs == rhs);

    /// <summary>
    /// When overridden in a derived class, returns the components that take part
    /// in equality and hash-code computation, in a stable order.
    /// </summary>
    /// <returns>The sequence of values that define this value object's identity.</returns>
    protected abstract IEnumerable<object?> GetEqualityMembers();

    /// <summary>
    /// Determines whether the specified object is a value object of the same
    /// type with an equal sequence of equality members.
    /// </summary>
    /// <param name="obj">The object to compare with the current value object.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is structurally equal;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is ValueObject other
        && this.GetType() == other.GetType()
        && this.GetEqualityMembers().SequenceEqual(other.GetEqualityMembers());

    /// <summary>
    /// Determines whether the specified value object is structurally equal to
    /// the current one.
    /// </summary>
    /// <param name="other">The value object to compare with, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the value objects are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(ValueObject? other) => this.Equals((object?)other);

    /// <summary>
    /// Returns a hash code combining the runtime type and every equality member,
    /// consistent with <see cref="Equals(object?)"/>.
    /// </summary>
    /// <returns>A hash code for the current value object.</returns>
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

/// <summary>
/// Base class for single-value value objects that wrap and validate one
/// underlying value. Equality is based solely on the wrapped <see cref="Value"/>.
/// </summary>
/// <typeparam name="TValue">Type of the wrapped value.</typeparam>
public abstract class ValueObject<TValue> : ValueObject
{
    /// <summary>
    /// Gets the wrapped value. The value is validated through
    /// <see cref="ValidateValue"/> whenever it is assigned.
    /// </summary>
    protected TValue Value
    {
        get;
        private set
        {
            this.ValidateValue(value);

            field = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValueObject{TValue}"/> class,
    /// validating the supplied value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    protected ValueObject(TValue value)
    {
        this.Value = value;
    }

    /// <summary>
    /// When overridden in a derived class, validates a candidate value and
    /// throws if it violates the value object's invariants.
    /// </summary>
    /// <param name="value">The candidate value to validate.</param>
    protected abstract void ValidateValue(TValue value);

    /// <summary>
    /// Returns the single equality member, which is the wrapped <see cref="Value"/>.
    /// </summary>
    /// <returns>A sequence containing the wrapped value.</returns>
    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Value;
    }

    /// <summary>
    /// Returns the string representation of the wrapped <see cref="Value"/>.
    /// </summary>
    /// <returns>The wrapped value's string form, or <see langword="null"/>.</returns>
    public override string? ToString() => this.Value?.ToString();
}
