using System.Reflection;

namespace DDD.Domain.Model;

/// <summary>
/// Base class for type-safe enumerations: richer, object-oriented alternatives
/// to <see langword="enum"/> that can carry behaviour and are declared as
/// <see langword="public static"/> fields on the derived type. Instances are
/// compared by their wrapped <see cref="Value"/>.
/// </summary>
/// <typeparam name="TValue">Type of the underlying value each member wraps.</typeparam>
/// <typeparam name="TEnumeration">
/// The concrete enumeration type deriving from this class (curiously recurring
/// generic pattern). Must expose a public parameterless constructor.
/// </typeparam>
public abstract class Enumeration<TValue, TEnumeration> : IEquatable<TEnumeration>
    where TValue : IEquatable<TValue>?
    where TEnumeration : Enumeration<TValue, TEnumeration>, new()
{
    /// <summary>
    /// Gets the default member of the enumeration, whose value is the derived
    /// type's <see cref="DefaultValue"/>.
    /// </summary>
    public static TEnumeration Default => new();

    /// <summary>
    /// Gets the underlying value this enumeration member wraps.
    /// </summary>
    protected TValue? Value { get; }

    /// <summary>
    /// When overridden in a derived class, gets the value assigned to the
    /// <see cref="Default"/> member.
    /// </summary>
    protected abstract TValue DefaultValue { get; }

    /// <summary>
    /// Initializes a new instance whose value is the derived type's
    /// <see cref="DefaultValue"/>.
    /// </summary>
    protected Enumeration()
    {
        this.Value = this.DefaultValue;
    }

    /// <summary>
    /// Initializes a new instance wrapping the supplied value.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    protected Enumeration(TValue? value)
    {
        this.Value = value;
    }

    /// <summary>
    /// Returns the given enumeration, or the <see cref="Default"/> member when
    /// it is <see langword="null"/>.
    /// </summary>
    /// <param name="enumeration">The enumeration to collate, possibly <see langword="null"/>.</param>
    /// <returns>The supplied enumeration, or <see cref="Default"/> if it was <see langword="null"/>.</returns>
    public static TEnumeration CollateNull(TEnumeration? enumeration) =>
        enumeration ?? Enumeration<TValue, TEnumeration>.Default;

    /// <summary>
    /// Enumerates all members declared as public static fields on the derived
    /// enumeration type.
    /// </summary>
    /// <returns>The declared enumeration members.</returns>
    public static IEnumerable<TEnumeration?> GetValues() =>
        typeof(TEnumeration)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(v => v.FieldType == typeof(TEnumeration))
            .Select(v => (TEnumeration?)v.GetValue(null));

    /// <summary>
    /// Enumerates the names of all members declared as public static fields on
    /// the derived enumeration type.
    /// </summary>
    /// <returns>The declared member names.</returns>
    public static IEnumerable<string> GetNames() =>
        typeof(TEnumeration)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(v => v.FieldType == typeof(TEnumeration))
            .Select(v => v.Name);

    /// <summary>
    /// Returns a hash code derived from the runtime type and the wrapped value.
    /// </summary>
    /// <returns>A hash code for the current enumeration member.</returns>
    public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Value);

    /// <summary>
    /// Determines whether the specified object is an enumeration member of the
    /// same type with an equal value.
    /// </summary>
    /// <param name="obj">The object to compare with the current member.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="obj"/> is an equal member;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public override bool Equals(object? obj) =>
        obj is TEnumeration enumeration
        && obj.GetType() == this.GetType()
        && this.Equals(enumeration);

    /// <summary>
    /// Determines whether the specified enumeration member wraps an equal value.
    /// </summary>
    /// <param name="other">The member to compare with, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the members are equal; otherwise <see langword="false"/>.
    /// </returns>
    public bool Equals(TEnumeration? other) =>
        AreValuesEqual(this.Value, other is null ? default : other.Value);

    /// <summary>
    /// Determines whether two enumeration members wrap equal values.
    /// </summary>
    /// <param name="lhs">The left-hand member, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand member, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if both are <see langword="null"/> or equal;
    /// otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator ==(
        Enumeration<TValue, TEnumeration>? lhs,
        Enumeration<TValue, TEnumeration>? rhs
    ) => lhs is null ? rhs is null : lhs.Equals(rhs);

    /// <summary>
    /// Determines whether two enumeration members wrap different values.
    /// </summary>
    /// <param name="lhs">The left-hand member, or <see langword="null"/>.</param>
    /// <param name="rhs">The right-hand member, or <see langword="null"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the members differ; otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(
        Enumeration<TValue, TEnumeration>? lhs,
        Enumeration<TValue, TEnumeration>? rhs
    ) => !(lhs == rhs);

    /// <summary>
    /// Returns the string representation of the wrapped value.
    /// </summary>
    /// <returns>The value's string form, or <see langword="null"/>.</returns>
    public override string? ToString() => this.Value?.ToString();

    /// <summary>
    /// Converts a raw value to the matching enumeration member.
    /// </summary>
    /// <param name="value">The raw value to resolve to a declared member.</param>
    /// <returns>The enumeration member whose value equals <paramref name="value"/>.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when no declared member wraps the given value.
    /// </exception>
    public static implicit operator Enumeration<TValue, TEnumeration>(TValue value) =>
        GetValues().FirstOrDefault(v => v is not null && AreValuesEqual(v.Value, value))
        ?? throw new ArgumentException($"Wrong {typeof(TEnumeration).Name} value.");

    private static bool AreValuesEqual(TValue? lhsValue, TValue? rhsValue) =>
        EqualityComparer<TValue?>.Default.Equals(lhsValue, rhsValue);

    /// <summary>
    /// Extracts the underlying value wrapped by an enumeration member.
    /// </summary>
    /// <param name="value">The enumeration member, or <see langword="null"/>.</param>
    /// <returns>
    /// The wrapped value, or the default of <typeparamref name="TValue"/> when
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </returns>
    public static implicit operator TValue?(Enumeration<TValue, TEnumeration>? value) =>
        value is null ? default : value.Value;
}
