using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

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
            if(other == null || other.GetType() != this.GetType())
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

                if(membersValues.Current != null
                    && !membersValues.Current.Equals(otherMembersValues.Current))
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

        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 2893249;
                hash = hash * 1674319 + this.GetType().GetHashCode();

                foreach(var memberValue in this.GetEqualityMembers())
                {
                    hash = hash * 1674319 + ((memberValue != null) ? memberValue.GetHashCode() : 0);
                }

                return hash;
            }
        }
    }

    public abstract class ValueObject<TMembersValidator> : ValueObject
        where TMembersValidator : IMembersValidator, new()
    {
        protected TMembersValidator Validator { get; }

        protected ValueObject()
        {
            this.Validator = new TMembersValidator();
        }
    }
}