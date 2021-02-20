using System;

namespace DDD.Model
{
    public abstract class Entity<TIdentifier> : IEntity<TIdentifier>, IEquatable<Entity<TIdentifier>>
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
            return this.Equals(obj as Entity<TIdentifier>);
        }

        public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Id);

        public bool Equals(Entity<TIdentifier> other)
        {
            return other?.GetType() == this.GetType()
                && this.Id.Equals(((Entity<TIdentifier>)other).Id);
        }
    }

    public abstract class Entity<TIdentifier, TValidatedObject, TValidator> : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        protected Entity(TIdentifier id, TValidatedObject validatedObject) : base(id)
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