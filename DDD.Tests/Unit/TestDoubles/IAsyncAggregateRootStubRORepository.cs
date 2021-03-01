using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAsyncAggregateRootStubRORepository : IAsyncReadOnlyRepository<AggregateRootStub, string>
    {
    }
}