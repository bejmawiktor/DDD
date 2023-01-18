using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class IntIdFake : Identifier<int, IntIdFake>
    {
        public IntIdFake(int value) : base(value)
        {
        }

        protected override void ValidateValue(int value)
        {
        }
    }
}