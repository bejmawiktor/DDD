using System;

namespace DDD.Model
{
    public abstract class Aggregate<TIdentifier> : Entity<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        protected Aggregate(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class Aggregate<TIdentifier, TValidatedObject, TValidator> : Aggregate<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        protected Aggregate(TIdentifier id, TValidatedObject validatedObject) : base(id)
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