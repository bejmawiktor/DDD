using System;

namespace DDD.Model
{
    public abstract class AggregateRoot<TIdentifier> : Entity<TIdentifier>, IAggregateRoot<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        protected AggregateRoot(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class AggregateRoot<TIdentifier, TValidatedObject, TValidator> : AggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        protected AggregateRoot(TIdentifier id, TValidatedObject validatedObject) : base(id)
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