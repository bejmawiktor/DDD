using DDD.Application.Model;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStub : IAggregateRootDto<AggregateRootStub, string>
    {
        public string Id { get; }
        public string? Name { get; }

        public AggregateRootDtoStub(string id, string? name = null)
        {
            this.Id = id;
            this.Name = name;
        }

        AggregateRootStub IDomainObjectDto<AggregateRootStub>.ToDomainObject()
        {
            return new AggregateRootStub(this.Id, this.Name);
        }
    }
}