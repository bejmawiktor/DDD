using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class ValidatedAggregateRootFake : AggregateRoot<string, int?, ValidatorFake>
{
    public ValidatedAggregateRootFake(string id, int? field1)
        : base(id, field1) { }
}
