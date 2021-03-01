using DDD.Tests.Unit.Application.TestDoubles;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application.Persistence
{
    [TestFixture]
    public class AsyncWirteOnlyRepositoryAdapterTest
    {
        [Test]
        public async Task TestAddAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsSet()
        {
            var dtoRepository = new AsyncAggregateRootDtoStubWORepository(new List<AggregateRootDtoStub>());
            IAsyncAggregateRootStubWORepository writeOnlyRepository = new AsyncWriteOnlyRepositoryAdapter(dtoRepository);

            await writeOnlyRepository.AddAsync(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos[0].Id, Is.EqualTo(new AggregateRootDtoStub("1").Id));
        }

        [Test]
        public async Task TestRemoveAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsRemoved()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AsyncAggregateRootDtoStubWORepository(aggregateRootDtosStubs);
            IAsyncAggregateRootStubWORepository writeOnlyRepository = new AsyncWriteOnlyRepositoryAdapter(dtoRepository);

            await writeOnlyRepository.RemoveAsync(new AggregateRootStub("1"));

            Assert.That(dtoRepository.Dtos, Is.Empty);
        }

        [Test]
        public async Task TestUpdateAsync_WhenAggregateRootDtoGiven_ThenAggregateRootIsUpdated()
        {
            var aggregateRootDtosStubs = new List<AggregateRootDtoStub>()
            {
                new AggregateRootDtoStub("1")
            };
            var dtoRepository = new AsyncAggregateRootDtoStubWORepository(aggregateRootDtosStubs);
            IAsyncAggregateRootStubWORepository writeOnlyRepository = new AsyncWriteOnlyRepositoryAdapter(dtoRepository);

            await writeOnlyRepository.UpdateAsync(new AggregateRootStub("1", "MyName"));

            Assert.That(dtoRepository.Dtos[0].Name, Is.EqualTo("MyName"));
        }
    }
}