using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public interface IAggregateRootStubRepository : IRepository<AggregateRootStub, string> { }
}
