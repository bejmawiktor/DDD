using System;

namespace DDD.Model
{
    public interface IEntity<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier Id { get; }
    }
}