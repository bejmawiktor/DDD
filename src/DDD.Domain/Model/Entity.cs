﻿using System;

namespace DDD.Domain.Model
{
    public abstract class Entity<TIdentifier> : IEntity<TIdentifier>
        where TIdentifier : notnull, IEquatable<TIdentifier>
    {
        private TIdentifier id;

        public TIdentifier Id
        {
            get => id;
            protected set => id = value ?? throw new ArgumentNullException(nameof(this.id));
        }

        protected Entity(TIdentifier id)
        {
            this.Id = id;
        }

        public static bool operator ==(Entity<TIdentifier> lhs, Entity<TIdentifier> rhs)
        {
            if (lhs is null && rhs is null)
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(Entity<TIdentifier> lhs, Entity<TIdentifier> rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object? obj)
        {
            Entity<TIdentifier>? other = obj as Entity<TIdentifier>;

            return this.GetType().Equals(other?.GetType()) && this.Id.Equals(other!.Id);
        }

        public override int GetHashCode() => HashCode.Combine(this.GetType(), this.Id);
    }

    public abstract class Entity<TIdentifier, TValidatedObject, TValidator> : Entity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TValidator : IValidator<TValidatedObject>, new()
    {
        protected TValidator Validator { get; }

        protected Entity(TIdentifier id, TValidatedObject validatedObject)
            : base(id)
        {
            this.Validator = new TValidator();

            this.Validator.Validate(validatedObject);
        }
    }
}
