using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStubRORepository : IReadOnlyDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AggregateRootDtoStubRORepository(List<AggregateRootDtoStub> dtos)
        {
            this.Dtos = dtos;
        }

        public AggregateRootDtoStub Get(string identifier)
        {
            return this.Dtos.FirstOrDefault(d => d.Id == identifier);
        }

        public IEnumerable<AggregateRootDtoStub> Get(Pagination pagination = null)
        {
            return this.Dtos;
        }
    }
}