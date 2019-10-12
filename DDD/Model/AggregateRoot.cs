using System;
using System.Runtime.CompilerServices;

namespace DDD.Model
{
    public abstract class AggregateRoot<TIdentifier> : Aggregate<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        public AggregateRoot(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class AggregateRoot<TIdentifier, TValidatedObject, TValidator> : AggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        public AggregateRoot(TIdentifier id, TValidatedObject validatedObject) : base(id)
        {
            this.Validator = new TValidator();

            if(default(TValidatedObject) == null && validatedObject == null)
            {
                throw new ArgumentNullException(nameof(validatedObject));
            }

            this.Validator.Validate(validatedObject);
        }
    }
}