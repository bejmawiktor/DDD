using System;
using System.Runtime.CompilerServices;

namespace DDD.Model
{
    public abstract class Entity<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        public TIdentifier Id { get; protected set; }

        protected Entity(TIdentifier id)
        {
            if(default(TIdentifier) == null && id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
        }

        public static bool operator ==(
            Entity<TIdentifier> lhs,
            Entity<TIdentifier> rhs)
        {
            if(lhs is null && rhs is null)
            {
                return true;
            }

            if(lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(
            Entity<TIdentifier> lhs,
            Entity<TIdentifier> rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj)
        {
            return obj?.GetType() == this.GetType()
                && this.Id.Equals(((Entity<TIdentifier>)obj).Id);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return 2108858624 + this.GetType().GetHashCode() + this.Id.GetHashCode();
            }
        }
    }

    public abstract class Entity<TIdentifier, TValidatedMembersTuple, TValidator> : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidatedMembersTuple : ITuple
        where TValidator : IValidator<TValidatedMembersTuple>, new()
    {
        protected TValidator Validator { get; }

        protected Entity(TIdentifier id, TValidatedMembersTuple validatedMembers) : base(id)
        {
            this.Validator = new TValidator();
            this.Validator.Validate(validatedMembers);
        }
    }
}