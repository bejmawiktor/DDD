using DDD.Persistence;

namespace DDD.Tests.Unit.TestDoubles
{
    public interface IAggregateRootStubWORepository : IWriteOnlyRepository<AggregateRootStub, string>
    {
    }
}