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

    public abstract class AggregateRoot<TIdentifier, TValidatedMembersTuple, TValidator> : AggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidatedMembersTuple : ITuple
        where TValidator : IValidator<TValidatedMembersTuple>, new()
    {
        protected TValidator Validator { get; }

        public AggregateRoot(TIdentifier id, TValidatedMembersTuple validatedMembers) : base(id)
        {
            this.Validator = new TValidator();
            this.Validator.Validate(validatedMembers);
        }
    }
}