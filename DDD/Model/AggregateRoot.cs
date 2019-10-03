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
}