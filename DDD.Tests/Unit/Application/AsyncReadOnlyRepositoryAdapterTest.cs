using NUnit.Framework;
using DDD.Tests.Unit.TestDoubles;
using DDD.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application
{
    [TestFixture]
    public class AsyncAsyncReadOnlyRepositoryAdapterTest
    {
        [Test]
        public async Task TestGetAsync_WhenIdentifierGiven_ThenAggregateRootIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AsyncAggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRORepository readOnlyRepository = new AsyncReadOnlyRepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = await readOnlyRepository.GetAsync("1");

            Assert.That(aggregateRootStub, Is.EqualTo(new AggregateRootStub("1")));
        }

        [Test]
        public async Task TestGetAsync_WhenPaginationGiven_ThenAggregateRootsAreReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AsyncAggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRORepository readOnlyRepository = new AsyncReadOnlyRepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = await readOnlyRepository.GetAsync(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.EqualTo(new AggregateRootStub[] { new AggregateRootStub("1") }));
        }

        [Test]
        public async Task TestGetAsync_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AsyncAggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAsyncAggregateRootStubRORepository readOnlyRepository = new AsyncReadOnlyRepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = await readOnlyRepository.GetAsync("2");

            Assert.That(aggregateRootStub, Is.Null);
        }
        
        [Test]
        public async Task TestGetAsync_WhenNullIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var dtoRepository = new AsyncAggregateRootDtoStubRORepository(null);
            IAsyncAggregateRootStubRORepository readOnlyRepository = new AsyncReadOnlyRepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = await readOnlyRepository.GetAsync(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.Null);
        }
    }
}