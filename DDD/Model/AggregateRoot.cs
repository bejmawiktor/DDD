using System;

namespace DDD.Model
{
    public abstract class AggregateRoot<TIdentifier> : Aggregate<TIdentifier>
         where TIdentifier : IEquatable<TIdentifier>
    {
        public AggregateRoot(TIdentifier id) : base(id)
        {
        }
    }

    public abstract class AggregateRoot<TIdentifier, TMembersValidator> : AggregateRoot<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
        where TMembersValidator : IMembersValidator, new()
    {
        protected TMembersValidator Validator { get; }

        public AggregateRoot(TIdentifier id) : base(id)
        {
            this.Validator = new TMembersValidator();
        }
    }
}