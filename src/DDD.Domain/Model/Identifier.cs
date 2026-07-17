namespace DDD.Domain.Model;

/// <summary>
/// Base class for strongly typed identifiers modelled as value objects. Wraps
/// an underlying value, validates it on assignment and provides value-based
/// equality, so each entity type can have its own dedicated identifier type.
/// </summary>
/// <typeparam name="TIdentifierValue">
/// Type of the underlying identifier value (for example <see cref="System.Guid"/>
/// or <see cref="string"/>). Must be non-nullable and comparable for equality.
/// </typeparam>
/// <typeparam name="TDeriviedIdentifier">
/// The concrete identifier type deriving from this class (curiously recurring
/// generic pattern), used to type the equality contract.
/// </typeparam>
public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier>
    : ValueObject,
        IEquatable<TDeriviedIdentifier>
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
    where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier>
{
    /// <summary>
    /// Gets the underlying identifier value. Assigning the value rejects
    /// <see langword="null"/> and runs <see cref="ValidateValue"/>.
    /// </summary>
    public TIdentifierValue Value
    {
        get;
        private set
        {
            ArgumentNullException.ThrowIfNull(value);
            this.ValidateValue(value);

            field = value;
        }
    }

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="Identifier{TIdentifierValue, TDeriviedIdentifier}"/> class with
    /// the supplied value.
    /// </summary>
    /// <param name="value">The underlying identifier value. Cannot be <see langword="null"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    protected Identifier(TIdentifierValue value)
        : base()
    {
        this.Value = value;
    }

    /// <summary>
    /// Determines whether the specified identifier wraps an equal value.
    /// </summary>
    /// <param name="other">The identifier to compare with, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the identifiers are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TDeriviedIdentifier? other) => base.Equals(other);

    /// <summary>
    /// When overridden in a derived class, validates the identifier value and
    /// throws if it violates the identifier's invariants.
    /// </summary>
    /// <param name="value">The candidate value to validate.</param>
    protected abstract void ValidateValue(TIdentifierValue value);

    /// <summary>
    /// Returns the single equality member, which is the wrapped <see cref="Value"/>.
    /// </summary>
    /// <returns>A sequence containing the wrapped value.</returns>
    protected sealed override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Value;
    }

    /// <summary>
    /// Returns the string representation of the wrapped <see cref="Value"/>.
    /// </summary>
    /// <returns>The value's string form, or <see langword="null"/>.</returns>
    public override string? ToString() => this.Value.ToString();
}
