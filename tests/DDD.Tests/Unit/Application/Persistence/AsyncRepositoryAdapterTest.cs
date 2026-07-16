using DDD.Tests.Unit.Application.TestDoubles;

namespace DDD.Tests.Unit.Application.Persistence;

public class AsyncRepositoryAdapterTest
{
    [Test]
    public async Task TestGetAsync_WhenIdentifierGiven_ThenAggregateRootIsReturned()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(dtoRepository);

        AggregateRootStub? aggregateRootStub = await repository.GetAsync("1");

        await Assert.That(aggregateRootStub).IsEqualTo(new AggregateRootStub("1"));
    }

    [Test]
    public async Task TestGetAsync_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(dtoRepository);

        AggregateRootStub? aggregateRootStub = await repository.GetAsync("2");

        await Assert.That(aggregateRootStub).IsNull();
    }

    [Test]
    public async Task TestAddAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
    {
        AsyncAggregateRootDtoStubRepository dtoRepository = new([]);
        IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(dtoRepository);

        await repository.AddAsync(new AggregateRootStub("1"));

        await Assert.That(dtoRepository.Dtos![0].Id).IsEqualTo(new AggregateRootDtoStub("1").Id);
    }

    [Test]
    public async Task TestRemoveAsync_WhenIdentifierGiven_ThenAggregateRootIsRemoved()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(dtoRepository);

        await repository.RemoveAsync(new AggregateRootStub("1"));

        await Assert.That(dtoRepository.Dtos).IsEmpty();
    }

    [Test]
    public async Task TestUpdateAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(dtoRepository);

        await repository.UpdateAsync(new AggregateRootStub("1", "MyName"));

        await Assert.That(dtoRepository.Dtos![0].Name).IsEqualTo("MyName");
    }
}
