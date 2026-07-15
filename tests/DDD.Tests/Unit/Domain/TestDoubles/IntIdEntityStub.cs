using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class IntIdEntityStub(int id) : Entity<int>(id)
{
}
