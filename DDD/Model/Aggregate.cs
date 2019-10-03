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
}