using System;
using System.Collections.Generic;

namespace DDD.Model
{
    public abstract class ValueObject : IEquatable<ValueObject>
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

        public bool Equals(ValueObject other)
        {
            if(this.GetType() != other?.GetType())
            {
                return false;
            }

            IEnumerator<object> membersValues = this.GetEqualityMembers().GetEnumerator();
            IEnumerator<object> otherMembersValues = other.GetEqualityMembers().GetEnumerator();
            bool moveNextResult = membersValues.MoveNext();
            bool otherMoveNextResult = otherMembersValues.MoveNext();

            for(; moveNextResult && otherMoveNextResult
                ; moveNextResult = membersValues.MoveNext(),
                    otherMoveNextResult = otherMembersValues.MoveNext())
            {
                if(membersValues.Current == null
                    ^ otherMembersValues.Current == null)
                {
                    return false;
                }

                if(membersValues.Current?.Equals(otherMembersValues.Current) == false)
                {
                    return false;
                }
            }

            return !moveNextResult && !otherMoveNextResult;
        }

        protected abstract IEnumerable<object> GetEqualityMembers();

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ValueObject);
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