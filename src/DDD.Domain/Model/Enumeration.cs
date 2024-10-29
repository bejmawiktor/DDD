using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Domain.Model;

public abstract class Enumeration<TValue, TEnumeration> : IEquatable<TEnumeration>
    where TValue : IEquatable<TValue>?
    where TEnumeration : Enumeration<TValue, TEnumeration>, new()
{
    public static TEnumeration Default => new();

    protected TValue? Value { get; }
    protected abstract TValue DefaultValue { get; }

    protected Enumeration()
    {
        this.Value = this.DefaultValue;
    }

    protected Enumeration(TValue? value)
    {
        this.Value = value;
    }

    public static TEnumeration CollateNull(TEnumeration? enumeration) =>
        enumeration ?? Enumeration<TValue, TEnumeration>.Default;

    public static IEnumerable<TEnumeration?> GetValues()
    {
        return typeof(TEnumeration)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(v => v.FieldType == typeof(TEnumeration))
            .Select(v => (TEnumeration?)v.GetValue(null));
    }

    public static IEnumerable<string> GetNames()
    {
        return typeof(TEnumeration)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Where(v => v.FieldType == typeof(TEnumeration))
            .Select(v => v.Name);
    }

    public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Value);

    public override bool Equals(object? other) =>
        (other?.GetType()) == this.GetType() && this.Equals(other as TEnumeration);

    public bool Equals(TEnumeration? other)
    {
        return !(this.Value is null ^ (other is null || other.Value is null))
            && (
                (this.Value is null && (other is null || other.Value is null))
                || this.Value!.Equals(other!.Value)
            );
    }

    public static bool operator ==(
        Enumeration<TValue, TEnumeration> lhs,
        Enumeration<TValue, TEnumeration> rhs
    ) => (lhs is null && rhs is null) || (lhs is not null && rhs is not null && lhs.Equals(rhs));

    public static bool operator !=(
        Enumeration<TValue, TEnumeration> lhs,
        Enumeration<TValue, TEnumeration> rhs
    ) => !(lhs == rhs);

    public override string? ToString() => this.Value?.ToString();

    public static implicit operator Enumeration<TValue, TEnumeration>(TValue value) =>
        GetValues().FirstOrDefault(v => v is not null && AreValuesEqual(v.Value, value))
        ?? throw new ArgumentException($"Wrong {typeof(TEnumeration).Name} value.");

    private static bool AreValuesEqual(TValue? lhsValue, TValue? rhsValue) =>
        !(lhsValue is null ^ rhsValue is null)
        && ((lhsValue is null && rhsValue is null) || lhsValue!.Equals(rhsValue));

    public static implicit operator TValue?(Enumeration<TValue, TEnumeration>? value) =>
        value is null ? default : value.Value;
}
