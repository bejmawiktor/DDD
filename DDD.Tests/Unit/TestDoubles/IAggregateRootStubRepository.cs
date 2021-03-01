using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAggregateRootStubRepository : IRepository<AggregateRootStub, string>
    {
    }
}