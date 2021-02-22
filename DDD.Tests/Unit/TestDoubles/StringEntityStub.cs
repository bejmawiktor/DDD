using DDD.Model;

namespace DDD.Tests.Unit.TestDoubles
{
    public class StringEntityStub : Entity<string>
    {
        public StringEntityStub(string id) : base(id)
        {
        }
    }
}