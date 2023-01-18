using System;

namespace DDD.Domain.Model
{
    public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier>
    : ValueObject<TIdentifierValue>, IEquatable<TDeriviedIdentifier>
       where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
       where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier>
    {
        public new TIdentifierValue Value => base.Value!;

        protected Identifier(TIdentifierValue value) : base(value)
        {
        }

        internal override sealed void PrevalidateValue(TIdentifierValue value)
            => ArgumentNullException.ThrowIfNull(value);

        public bool Equals(TDeriviedIdentifier? other)
        {
            return base.Equals(other);
        }
    }
}