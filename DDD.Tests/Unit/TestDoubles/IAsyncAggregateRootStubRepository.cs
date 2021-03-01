using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAsyncAggregateRootStubRepository : IAsyncRepository<AggregateRootStub, string>
    {
    }
}