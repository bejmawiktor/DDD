using System;
using System.Collections.Generic;

namespace DDD.Domain.Model
{
    public abstract class Identifier<TIdentifier, TDeriviedIdentifier> : ValueObject, IEquatable<TDeriviedIdentifier>
       where TIdentifier : IEquatable<TIdentifier>
       where TDeriviedIdentifier : Identifier<TIdentifier, TDeriviedIdentifier>
    {
        public TIdentifier Value { get; }

        protected Identifier(TIdentifier value)
        {
            if(default(TIdentifier) == null && value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.ValidateValue(value);

            this.Value = value;
        }

        protected abstract void ValidateValue(TIdentifier value);

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }

        public bool Equals(TDeriviedIdentifier other)
        {
            return base.Equals(other);
        }
    }
}