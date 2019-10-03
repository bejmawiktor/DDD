using System;
using System.Collections.Generic;

namespace DDD.Model
{
    public abstract class Identifier<TIdentifier, TIdentifierChild> : ValueObject, IEquatable<TIdentifierChild>
        where TIdentifier : IEquatable<TIdentifier>
        where TIdentifierChild : Identifier<TIdentifier, TIdentifierChild>
    {
        public TIdentifier Value { get; }

        public Identifier(TIdentifier value)
        {
            if(default(TIdentifier) == null && value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Value;
        }

        public bool Equals(TIdentifierChild other)
        {
            return base.Equals(other);
        }
    }
}