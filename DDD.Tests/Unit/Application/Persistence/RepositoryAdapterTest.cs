using DDD.Domain.Persistence;
using DDD.Tests.Unit.Application.TestDoubles;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Application.Persistence
{
    [TestFixture]
    public class RepositoryAdapterTest
    {
        [Test]
        public void TestGet_WhenIdentifierGiven_ThenAggregateRootIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRepository(aggregateRootDtosStubs);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = repository.Get("1");

            Assert.That(aggregateRootStub, Is.EqualTo(new AggregateRootStub("1")));
        }

        [Test]
        public void TestGet_WhenPaginationGiven_ThenAggregateRootsAreReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRepository(aggregateRootDtosStubs);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = repository.Get(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.EqualTo(new AggregateRootStub[] { new AggregateRootStub("1") }));
        }

        [Test]
        public void TestGet_WhenNullAggregateRootDtoIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRepository(aggregateRootDtosStubs);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            AggregateRootStub aggregateRootStub = repository.Get("2");

            Assert.That(aggregateRootStub, Is.Null);
        }

        [Test]
        public void TestGet_WhenNullIsReturnedFromDtoRepository_ThenNullIsReturned()
        {
            var dtoRepository = new AggregateRootDtoStubRepository(null);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            IEnumerable<AggregateRootStub> aggregateRoots = repository.Get(new Pagination(1, 100));

            Assert.That(aggregateRoots, Is.Null);
        }

        [Test]
        public void TestAdd_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
        {
            var dtoRepository = new AggregateRootDtoStubRepository(new List<AggregateRootDtoStub>());
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            repository.Add(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos[0].Id, Is.EqualTo(new AggregateRootDtoStub("1").Id));
        }

        [Test]
        public void TestRemove_WhenIdentifierGiven_ThenAggregateRootIsRemoved()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRepository(aggregateRootDtosStubs);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            repository.Remove("1");

            Assert.That(dtoRepository.Dtos, Is.Empty);
        }

        [Test]
        public void TestUpdate_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubRepository(aggregateRootDtosStubs);
            IAggregateRootStubRepository repository = new RepositoryAdapter(dtoRepository);

            repository.Update(new AggregateRootStub("1", "MyName"));

            Assert.That(dtoRepository.Dtos[0].Name, Is.EqualTo("MyName"));
        }
    }
}