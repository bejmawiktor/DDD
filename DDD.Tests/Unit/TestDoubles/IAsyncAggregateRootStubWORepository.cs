using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAsyncAggregateRootStubWORepository : IAsyncWriteOnlyRepository<AggregateRootStub, string>
    {
    }
}