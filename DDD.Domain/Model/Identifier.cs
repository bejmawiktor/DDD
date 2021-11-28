using System;

namespace DDD.Domain.Model
{
    public abstract class Identifier<TIdentifier, TDeriviedIdentifier>
    : ValueObject<TIdentifier, TDeriviedIdentifier>, IEquatable<TDeriviedIdentifier>
       where TIdentifier : IEquatable<TIdentifier>
       where TDeriviedIdentifier : Identifier<TIdentifier, TDeriviedIdentifier>
    {
        public new TIdentifier Value => base.Value;

        protected Identifier(TIdentifier value) : base(value)
        {
        }

        internal override sealed void ValidateValueInternal(TIdentifier value)
        {
            if(default(TIdentifier) == null && value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }
}