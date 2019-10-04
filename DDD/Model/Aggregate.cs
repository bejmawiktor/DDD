using System;
using System.Runtime.CompilerServices;

namespace DDD.Model
{
    public abstract class Aggregate<TIdentifier> : Entity<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        public Aggregate(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class Aggregate<TIdentifier, TValidatedMembersTuple, TValidator> : Aggregate<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidatedMembersTuple : ITuple
        where TValidator : IValidator<TValidatedMembersTuple>, new()
    {
        protected TValidator Validator { get; }

        public Aggregate(TIdentifier id, TValidatedMembersTuple validatedMembers) : base(id)
        {
            this.Validator = new TValidator();
            this.Validator.Validate(validatedMembers);
        }
    }
}