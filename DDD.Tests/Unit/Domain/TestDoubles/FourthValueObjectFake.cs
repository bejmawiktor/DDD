using DDD.Domain.Model;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class FourthValueObjectFake : ValueObject
    {
        public int Field1 { get; }
        public double Field2 { get; }
        public string Field3 { get; }
        public bool Field4 { get; }

        public FourthValueObjectFake(int field1, double field2, string field3, bool field4)
        {
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
            this.Field4 = field4;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Field1;
            yield return this.Field2;
            if(this.Field4)
            {
                yield return this.Field3;
            }
        }
    }
}