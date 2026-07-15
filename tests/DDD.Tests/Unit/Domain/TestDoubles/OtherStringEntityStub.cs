using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class OtherStringEntityStub(string id) : Entity<string>(id)
{
}
