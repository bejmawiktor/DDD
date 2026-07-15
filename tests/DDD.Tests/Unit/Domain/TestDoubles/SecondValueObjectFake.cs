using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class SecondValueObjectFake(int field1, string field2) : ValueObject
{
    public int Field1 { get; } = field1;
    public string Field2 { get; } = field2;

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Field1;
        yield return this.Field2;
    }
}
