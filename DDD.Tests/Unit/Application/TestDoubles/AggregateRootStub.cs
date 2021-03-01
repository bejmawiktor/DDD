using DDD.Domain.Model;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootStub : AggregateRoot<string>
    {
        public string Name { get; }

        public AggregateRootStub(string id, string name = null) : base(id)
        {
            this.Name = name;
        }
    }
}