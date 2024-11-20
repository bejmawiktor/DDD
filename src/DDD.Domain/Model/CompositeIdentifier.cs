using System;
using System.Runtime.CompilerServices;

namespace DDD.Domain.Model;

public abstract class CompositeIdentifier<TTupleKeys, TDeriviedCompositeIdentifier>(
    TTupleKeys value
) : Identifier<TTupleKeys, TDeriviedCompositeIdentifier>(value)
    where TTupleKeys : notnull, ITuple, IEquatable<TTupleKeys>
    where TDeriviedCompositeIdentifier : CompositeIdentifier<
            TTupleKeys,
            TDeriviedCompositeIdentifier
        > { }
