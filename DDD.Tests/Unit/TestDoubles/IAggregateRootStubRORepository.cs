using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAggregateRootStubRORepository : IReadOnlyRepository<AggregateRootStub, string>
    {
    }
}