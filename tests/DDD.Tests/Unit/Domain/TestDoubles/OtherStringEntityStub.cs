using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles
{
    public class OtherStringEntityStub : Entity<string>
    {
        public OtherStringEntityStub(string id) : base(id)
        {
        }
    }
}