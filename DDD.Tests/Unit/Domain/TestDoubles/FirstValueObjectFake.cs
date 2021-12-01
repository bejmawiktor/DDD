using DDD.Domain.Model;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class FirstValueObjectFake : ValueObject
    {
        public int Field1 { get; }
        public string Field2 { get; }

        public FirstValueObjectFake(int field1, string field2)
        {
            this.Field1 = field1;
            this.Field2 = field2;
        }

        protected override IEnumerable<object> GetEqualityMembers()
        {
            yield return this.Field1;
            yield return this.Field2;
        }
    }
}