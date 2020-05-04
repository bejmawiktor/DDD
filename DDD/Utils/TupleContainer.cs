using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace DDD.Utils
{
    public class TupleContainer<TTuple> where TTuple : ITuple
    {
        public TTuple Tuple { get; }

        public TupleContainer(TTuple tuple)
        {
            this.Tuple = tuple ?? throw new ArgumentNullException(nameof(tuple));
        }

        public override bool Equals(object obj)
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