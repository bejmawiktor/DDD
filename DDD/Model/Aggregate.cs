using System;

namespace DDD.Model
{
    public abstract class Aggregate<TIdentifier> : Entity<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        public Aggregate(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class Aggregate<TIdentifier, TMembersValidator> : Aggregate<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TMembersValidator : IMembersValidator, new()
    {
        protected TMembersValidator Validator { get; }

        public Aggregate(TIdentifier id) : base(id)
        {
            this.Validator = new TMembersValidator();
        }
    }
}