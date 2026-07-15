using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class ThirdValueObjectFake(int field1, double field2, string? field3) : ValueObject
{
    public int Field1 { get; } = field1;
    public double Field2 { get; } = field2;
    public string? Field3 { get; } = field3;

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.Field1;
        yield return this.Field2;
        yield return this.Field3;
    }
}
