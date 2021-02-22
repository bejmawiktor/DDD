using DDD.Model;

namespace DDD.Tests.Unit.TestDoubles
{
    public class ValidatedEntityFake : Entity<string, int?, ValidatorFake>
    {
        public ValidatedEntityFake(string id, int? field1)
        : base(id, field1)
        {
        }
    }
}