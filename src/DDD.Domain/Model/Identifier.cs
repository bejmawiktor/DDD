using System;
using System.Collections.Generic;

namespace DDD.Domain.Model;

public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier>
    : ValueObject,
        IEquatable<TDeriviedIdentifier>
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
    where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier>
{
    public TIdentifierValue Value { get; }

    protected Identifier(TIdentifierValue value)
        : base()
    {
        ArgumentNullException.ThrowIfNull(value);
        this.ValidateValue(value);

        this.Value = value;
    }

    public bool Equals(TDeriviedIdentifier? other) => base.Equals(other);

    protected abstract void ValidateValue(TIdentifierValue value);

    protected sealed override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Value;
    }

    public override string? ToString() => this.Value.ToString();
}
