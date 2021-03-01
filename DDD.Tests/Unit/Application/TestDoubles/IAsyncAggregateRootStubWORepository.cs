using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public interface IAsyncAggregateRootStubWORepository : IAsyncWriteOnlyRepository<AggregateRootStub, string>
    {
    }
}