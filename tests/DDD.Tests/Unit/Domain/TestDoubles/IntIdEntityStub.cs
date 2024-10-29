using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class IntIdEntityStub : Entity<int>
{
    public IntIdEntityStub(int id)
        : base(id) { }
}
