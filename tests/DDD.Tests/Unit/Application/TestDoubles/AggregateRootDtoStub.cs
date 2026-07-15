using DDD.Application.Model;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class AggregateRootDtoStub(string id, string? name = null) : IAggregateRootDto<AggregateRootStub, string>
{
    public string Id { get; } = id;
    public string? Name { get; } = name;

    AggregateRootStub IDomainObjectDto<AggregateRootStub>.ToDomainObject() =>
        new(this.Id, this.Name);
}
