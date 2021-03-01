using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public interface IAsyncAggregateRootStubRORepository : IAsyncReadOnlyRepository<AggregateRootStub, string>
    {
    }
}