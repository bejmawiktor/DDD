using System;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Domain.Model
{
    public abstract class ValueObject : IDomainObject
    {
        public static bool operator ==(ValueObject lhs, ValueObject rhs)
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

        public static bool operator !=(ValueObject lhs, ValueObject rhs)
        {
            return !(lhs == rhs);
        }

        protected abstract IEnumerable<object?> GetEqualityMembers();

        public override bool Equals(object? obj)
        {
            if(this.GetType() != obj?.GetType())
            {
                return false;
            }

            var other = obj as ValueObject;

            return this.GetEqualityMembers()
                .SequenceEqual(other!.GetEqualityMembers());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2893249;
                hash = hash * 1674319 + this.GetType().GetHashCode();

                foreach(var memberValue in this.GetEqualityMembers())
                {
                    hash = hash * 1674319 + (memberValue?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }
    }

    public abstract class ValueObject<TValue> : ValueObject
    {
        protected TValue? Value { get; }

        protected ValueObject(TValue value)
        {
            this.PrevalidateValue(value);
            this.ValidateValue(value);

            this.Value = value;
        }

        internal virtual void PrevalidateValue(TValue value)
        {
        }

        protected abstract void ValidateValue(TValue value);

        protected override IEnumerable<object?> GetEqualityMembers()
        {
            yield return this.Value;
        }

        public override string ToString()
            => this.Value?.ToString() ?? string.Empty;
    }

    public abstract class ValueObject<TValidatedObject, TValidator> : ValueObject
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        protected ValueObject(TValidatedObject validatedObject)
        {
            this.Validator = new TValidator();

            this.Validator.Validate(validatedObject);
        }
    }
}