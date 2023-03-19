using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class NullableSignleValueValueObjectFake : ValueObject<string?>
    {
        public NullableSignleValueValueObjectFake(string? value) : base(value)
        {
        }

        protected override void ValidateValue(string? value)
        {
        }
    }
}