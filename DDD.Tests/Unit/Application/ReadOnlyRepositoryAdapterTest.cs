using NUnit.Framework;
using DDD.Tests.Unit.TestDoubles;
using DDD.Persistence;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Application
{
    [TestFixture]
    public class ReadOnlyRepositoryAdapterTest
    {
        [Test]
        public void TestGet_WhenIdentifierGiven_ThenAggregateRootIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAggregateRootStubRORepository readOnlyRepository = new ReadOnlyRepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = readOnlyRepository.Get("1");

            Assert.That(aggregateRootStub, Is.EqualTo(new AggregateRootStub("1")));
        }

        [Test]
        public void TestGet_WhenPaginationGiven_ThenAggregateRootsAreReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAggregateRootStubRORepository readOnlyRepository = new ReadOnlyRepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = readOnlyRepository.Get(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.EqualTo(new AggregateRootStub[] { new AggregateRootStub("1") }));
        }

        [Test]
        public void TestGet_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRORepository(aggregateRootDtosStubs);
            IAggregateRootStubRORepository readOnlyRepository = new ReadOnlyRepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = readOnlyRepository.Get("2");

            Assert.That(aggregateRootStub, Is.Null);
        }
        
        [Test]
        public void TestGet_WhenNullIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var dtoRepository = new AggregateRootDtoStubRORepository(null);
            IAggregateRootStubRORepository readOnlyRepository = new ReadOnlyRepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = readOnlyRepository.Get(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.Null);
        }
    }
}