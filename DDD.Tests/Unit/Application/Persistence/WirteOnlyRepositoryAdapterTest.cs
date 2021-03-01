using DDD.Tests.Unit.Application.TestDoubles;
using NUnit.Framework;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Application.Persistence
{
    [TestFixture]
    public class WirteOnlyRepositoryAdapterTest
    {
        [Test]
        public void TestAdd_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
        {
            var dtoRepository = new AggregateRootDtoStubWORepository(new List<AggregateRootDtoStub>());
            IAggregateRootStubWORepository writeOnlyRepository = new WriteOnlyRepositoryAdapter(dtoRepository);

            writeOnlyRepository.Add(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos[0].Id, Is.EqualTo(new AggregateRootDtoStub("1").Id));
        }

        [Test]
        public void TestRemove_WhenAggregateRootDtoGiven_ThenAggregateRootIsRemoved()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubWORepository(aggregateRootDtosStubs);
            IAggregateRootStubWORepository writeOnlyRepository = new WriteOnlyRepositoryAdapter(dtoRepository);

            writeOnlyRepository.Remove(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos, Is.Empty);
        }

        [Test]
        public void TestUpdate_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AggregateRootDtoStubWORepository(aggregateRootDtosStubs);
            IAggregateRootStubWORepository writeOnlyRepository = new WriteOnlyRepositoryAdapter(dtoRepository);

            writeOnlyRepository.Update(new AggregateRootStub("1", "MyName"));

            Assert.That(dtoRepository.Dtos[0].Name, Is.EqualTo("MyName"));
        }
    }
}