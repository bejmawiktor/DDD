using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class StringEntityStub : Entity<string>
    {
        public StringEntityStub(string id) : base(id)
        {
        }
    }
}