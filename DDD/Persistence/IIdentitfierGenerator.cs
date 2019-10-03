using System;

namespace DDD.Persistence
{
    public interface IIdentifierGenerator<TIdentifier>
        where TIdentifier : IEquatable<TIdentifier>
    {
        TIdentifier NextIdentifier { get; }
    }
}