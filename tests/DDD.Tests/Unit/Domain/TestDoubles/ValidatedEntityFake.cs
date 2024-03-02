using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class ValidatedEntityFake : Entity<string, int?, ValidatorFake>
    {
        public ValidatedEntityFake(string id, int? field1)
            : base(id, field1) { }
    }
}
