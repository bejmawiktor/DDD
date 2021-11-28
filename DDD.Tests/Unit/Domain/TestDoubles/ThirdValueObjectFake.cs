using DDD.Domain.Model;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class ThirdValueObjectFake : ValueObject<ThirdValueObjectFake>
    {
        public int Field1 { get; }
        public double Field2 { get; }
        public string Field3 { get; }

        public ThirdValueObjectFake(int field1, double field2, string field3)
        {
            this.Field1 = field1;
            this.Field2 = field2;
            this.Field3 = field3;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Field1;
            yield return this.Field2;
            yield return this.Field3;
        }
    }
}