using DDD.Domain.Model;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class AggregateRootStub(string id, string? name = null) : AggregateRoot<string>(id)
{
    public string? Name { get; } = name;
}
