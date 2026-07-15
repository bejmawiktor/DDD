using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class IntIdFake(int value) : Identifier<int, IntIdFake>(value)
{
    protected override void ValidateValue(int value) { }
}
