using DDD.Application.Model.Converters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStubConverter
        : IAggregateRootDtoConverter<AggregateRootStub, string, AggregateRootDtoStub, string>
    {
        public AggregateRootDtoStub ToDto(AggregateRootStub aggregateRoot)
        {
            return new AggregateRootDtoStub(aggregateRoot.Id, aggregateRoot.Name);
        }

        public string ToDtoIdentifier(string identifier)
        {
            return identifier;
        }
    }
}
