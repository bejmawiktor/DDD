using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DDD.Model
{
    public abstract class Enumeration<TValue, TEnumeration> : IEquatable<TEnumeration>
        where TValue : IEquatable<TValue>
        where TEnumeration : Enumeration<TValue, TEnumeration>, new()
    {
        public static TEnumeration Default => new TEnumeration();

        protected TValue Value { get; }
        protected abstract TValue DefaultValue { get; }

        protected Enumeration()
        {
            this.Value = this.DefaultValue;
        }

        protected Enumeration(TValue value)
        {
            this.Value = value;
        }

        public static TEnumeration CollateNull(TEnumeration enumeration)
            => enumeration ?? Enumeration<TValue, TEnumeration>.Default;

        public static IEnumerable<TEnumeration> GetValues()
        {
            return typeof(TEnumeration)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(v => v.FieldType == typeof(TEnumeration))
                .Select(v => (TEnumeration)v?.GetValue(null));
        }

        public static IEnumerable<string> GetNames()
        {
            return typeof(TEnumeration)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
                .Where(v => v.FieldType == typeof(TEnumeration))
                .Select(v => v.Name);
        }

        public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Value);

        public override bool Equals(object other)
        {
            if(other?.GetType() != this.GetType())
            {
                return false;
            }

            return this.Equals(other as TEnumeration);
        }

        public bool Equals(TEnumeration other)
        {
            if(this.Value == null ^ (other == null || other.Value == null))
            {
                return false;
            }

            if(this.Value == null && (other == null || other.Value == null))
            {
                return true;
            }

            return this.Value.Equals(other.Value);
        }

        public static bool operator ==(
            Enumeration<TValue, TEnumeration> lhs,
            Enumeration<TValue, TEnumeration> rhs)
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
            Enumeration<TValue, TEnumeration> lhs,
            Enumeration<TValue, TEnumeration> rhs)
        {
            return !(lhs == rhs);
        }

        public override string ToString()
        {
            if(default(TValue) == null && this.Value == null)
            {
                return string.Empty;
            }

            return this.Value.ToString();
        }

        public static implicit operator Enumeration<TValue, TEnumeration>(TValue value)
            => GetValues().FirstOrDefault(v => v != null && AreValuesEqual(v.Value, value))
                ?? throw new ArgumentException($"Wrong {typeof(TEnumeration).Name} value.");

        private static bool AreValuesEqual(TValue lhsValue, TValue rhsValue)
        {
            if(lhsValue == null ^ rhsValue == null)
            {
                return false;
            }

            if(lhsValue == null && rhsValue == null)
            {
                return true;
            }

            return lhsValue.Equals(rhsValue);
        }

        public static implicit operator TValue(Enumeration<TValue, TEnumeration> value)
        {
            if(value == null)
            {
                return default;
            }

            return value.Value;
        }
    }
}