using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Persistence;
using DDD.Tests.Unit.Application.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Application.Persistence
{
    [TestFixture]
    public class AsyncRepositoryAdapterTest
    {
        [Test]
        public async Task TestGetAsync_WhenIdentifierGiven_ThenAggregateRootIsReturned()
        {
            List<AggregateRootDtoStub> aggregateRootDtosStubs =
                new() { new AggregateRootDtoStub("1") };
            AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            AggregateRootStub? aggregateRootStub = await repository.GetAsync("1");

            Assert.That(aggregateRootStub, Is.EqualTo(new AggregateRootStub("1")));
        }

        [Test]
        public async Task TestGetAsync_WhenPaginationGiven_ThenAggregateRootsAreReturned()
        {
            List<AggregateRootDtoStub> aggregateRootDtosStubs =
                new() { new AggregateRootDtoStub("1") };
            AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            IEnumerable<AggregateRootStub> aggregateRoots = await repository.GetAsync(
                new Pagination(1, 100)
            );

            Assert.That(
                aggregateRoots,
                Is.EqualTo(new AggregateRootStub[] { new AggregateRootStub("1") })
            );
        }

        [Test]
        public async Task TestGetAsync_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            List<AggregateRootDtoStub> aggregateRootDtosStubs =
                new() { new AggregateRootDtoStub("1") };
            AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            AggregateRootStub? aggregateRootStub = await repository.GetAsync("2");

            Assert.That(aggregateRootStub, Is.Null);
        }

        [Test]
        public async Task TestGetAsync_WhenNullIsReturnedFromDtoRepository_ThenEmptyEnumerableIsReturned()
        {
            AsyncAggregateRootDtoStubRepository dtoRepository = new(null);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            IEnumerable<AggregateRootStub> aggregateRoots = await repository.GetAsync(
                new Pagination(1, 100)
            );

            Assert.That(aggregateRoots, Is.Empty);
        }

        [Test]
        public async Task TestAddAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
        {
            AsyncAggregateRootDtoStubRepository dtoRepository =
                new(new List<AggregateRootDtoStub>());
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            await repository.AddAsync(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos![0].Id, Is.EqualTo(new AggregateRootDtoStub("1").Id));
        }

        [Test]
        public async Task TestRemoveAsync_WhenIdentifierGiven_ThenAggregateRootIsRemoved()
        {
            List<AggregateRootDtoStub> aggregateRootDtosStubs =
                new() { new AggregateRootDtoStub("1") };
            AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            await repository.RemoveAsync(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos, Is.Empty);
        }

        [Test]
        public async Task TestUpdateAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
        {
            List<AggregateRootDtoStub> aggregateRootDtosStubs =
                new() { new AggregateRootDtoStub("1") };
            AsyncAggregateRootDtoStubRepository dtoRepository = new(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRepository repository = new AsyncRepositoryAdapter(
                dtoRepository
            );

            await repository.UpdateAsync(new AggregateRootStub("1", "MyName"));

            Assert.That(dtoRepository.Dtos![0].Name, Is.EqualTo("MyName"));
        }
    }
}
