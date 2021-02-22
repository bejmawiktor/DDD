using DDD.Model;

namespace DDD.Tests.Unit.TestDoubles
{
    public class IntIdEntityStub : Entity<int>
    {
        public IntIdEntityStub(int id) : base(id)
        {
        }
    }
}