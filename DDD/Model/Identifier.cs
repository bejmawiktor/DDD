using System;
using System.Collections.Generic;

namespace DDD.Model
{
    public abstract class Identifier<TIdentifier> : ValueObject
        where TIdentifier : IEquatable<TIdentifier>
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
    }
}