using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class StringIdFake(string value) : Identifier<string, StringIdFake>(value)
{
    protected override void ValidateValue(string value)
    {
        if (value?.Length == 0)
        {
            throw new ArgumentException("Id could not be empty.");
        }
    }
}
