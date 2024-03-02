using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public interface IAsyncAggregateRootStubRepository
        : IAsyncRepository<AggregateRootStub, string> { }
}
