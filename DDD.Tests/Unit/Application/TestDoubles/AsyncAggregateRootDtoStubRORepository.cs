using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncAggregateRootDtoStubRORepository : IAsyncReadOnlyDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AsyncAggregateRootDtoStubRORepository(List<AggregateRootDtoStub> dtos)
        {
            this.Dtos = dtos;
        }

        public Task<AggregateRootDtoStub> GetAsync(string identifier)
        {
            return Task.FromResult(this.Dtos.FirstOrDefault(d => d.Id == identifier));
        }

        public Task<IEnumerable<AggregateRootDtoStub>> GetAsync(Pagination pagination = null)
        {
            return Task.FromResult(this.Dtos.AsEnumerable());
        }
    }
}