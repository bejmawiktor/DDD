using System;
using System.Runtime.CompilerServices;

namespace DDD.Domain.Model;

public abstract class CompositeIdentifier<TTupleKeys, TDeriviedCompositeIdentifier>
    : Identifier<TTupleKeys, TDeriviedCompositeIdentifier>
    where TTupleKeys : ITuple, IEquatable<TTupleKeys>
    where TDeriviedCompositeIdentifier : CompositeIdentifier<
            TTupleKeys,
            TDeriviedCompositeIdentifier
        >
{
    protected CompositeIdentifier(TTupleKeys value)
        : base(value) { }
}
