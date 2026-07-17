using DDD.Tests.Unit.Application.TestDoubles;

namespace DDD.Tests.Unit.Application.Persistence;

public class RepositoryAdapterTest
{
    [Test]
    public async Task TestGet_WhenIdentifierGiven_ThenAggregateRootIsReturned()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

        AggregateRootStub? aggregateRootStub = repository.Get("1");

        _ = await Assert.That(aggregateRootStub).IsEqualTo(new AggregateRootStub("1"));
    }

    [Test]
    public async Task TestGet_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

        AggregateRootStub? aggregateRootStub = repository.Get("2");

        _ = await Assert.That(aggregateRootStub).IsNull();
    }

    [Test]
    public async Task TestAdd_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
    {
        AggregateRootDtoStubRepository dtoRepository = new([]);
        IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

        repository.Add(new AggregateRootStub("1"));

        _ = await Assert
            .That(dtoRepository.Dtos![0].Id)
            .IsEqualTo(new AggregateRootDtoStub("1").Id);
    }

    [Test]
    public async Task TestRemove_WhenIdentifierGiven_ThenAggregateRootIsRemoved()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

        repository.Remove(new AggregateRootStub("1"));

        _ = await Assert.That(dtoRepository.Dtos).IsEmpty();
    }

    [Test]
    public async Task TestUpdate_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
    {
        List<AggregateRootDtoStub> aggregateRootDtosStubs = [new("1")];
        AggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
        IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

        repository.Update(new AggregateRootStub("1", "MyName"));

        _ = await Assert.That(dtoRepository.Dtos![0].Name).IsEqualTo("MyName");
    }
}
