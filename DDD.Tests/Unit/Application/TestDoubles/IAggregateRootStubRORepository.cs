using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public interface IAggregateRootStubRORepository : IReadOnlyRepository<AggregateRootStub, string>
    {
    }
}