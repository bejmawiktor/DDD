using System;

namespace DDD.Domain.Persistence;

public interface IIdentifierGenerator<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    TIdentifier NextIdentifier { get; }
}
