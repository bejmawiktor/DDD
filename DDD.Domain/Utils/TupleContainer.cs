using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DDD.Domain.Utils
{
    public class TupleContainer<TTuple> : ITuple where TTuple : notnull, ITuple
    {
        public TTuple Tuple { get; }

        public int Length => this.Tuple.Length;
        public object? this[int index] => this.Tuple[index];

        public TupleContainer(TTuple tuple)
        {
            this.Tuple = tuple ?? throw new ArgumentNullException(nameof(tuple));
        }

        public override bool Equals(object? obj)
        {
            return obj is TupleContainer<TTuple> container &&
                   EqualityComparer<TTuple>.Default.Equals(this.Tuple, container.Tuple);
        }

        public static bool operator ==(TupleContainer<TTuple> lhs, TupleContainer<TTuple> rhs)
        {
            if(lhs is null && rhs is null)
            {
                return true;
            }

            if(lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator !=(TupleContainer<TTuple> lhs, TupleContainer<TTuple> rhs)
            => !(lhs == rhs);

        public override int GetHashCode() => HashCode.Combine(this.Tuple);

        public static implicit operator TupleContainer<TTuple>(TTuple tuple)
            => new TupleContainer<TTuple>(tuple);

        public static implicit operator TTuple(TupleContainer<TTuple> tupleContainer)
            => tupleContainer.Tuple;
    }
}