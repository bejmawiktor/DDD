using DDD.Model;

namespace DDD.Tests.Unit.TestDoubles
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