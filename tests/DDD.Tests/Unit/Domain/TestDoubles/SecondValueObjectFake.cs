using System.Collections.Generic;
using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class SecondValueObjectFake : ValueObject
{
    public int Field1 { get; }
    public string Field2 { get; }

    public SecondValueObjectFake(int field1, string field2)
    {
        this.Field1 = field1;
        this.Field2 = field2;
    }

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Field1;
        yield return this.Field2;
    }
}
