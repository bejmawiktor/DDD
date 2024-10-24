using DDD.Application.Model.Converters;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class AggregateRootDtoStubConverter
    : IAggregateRootDtoConverter<AggregateRootStub, string, AggregateRootDtoStub, string>
{
    public AggregateRootDtoStub ToDto(AggregateRootStub aggregateRoot) => new(aggregateRoot.Id, aggregateRoot.Name);

    public string ToDtoIdentifier(string identifier) => identifier;
}