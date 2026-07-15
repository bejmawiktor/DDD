using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class NullableSignleValueValueObjectFake(string? value) : ValueObject<string?>(value)
{
    protected override void ValidateValue(string? value) { }
}
