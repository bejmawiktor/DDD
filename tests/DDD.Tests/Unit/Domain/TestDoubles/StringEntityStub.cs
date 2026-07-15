using DDD.Domain.Model;

namespace DDD.Tests.Unit.Domain.TestDoubles;

public class StringEntityStub(string id) : Entity<string>(id)
{
    public new string Id
    {
        get => base.Id;
        set => base.Id = value;
    }
}
