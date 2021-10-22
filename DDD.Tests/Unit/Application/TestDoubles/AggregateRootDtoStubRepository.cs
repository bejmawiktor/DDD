using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStubRepository : IDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AggregateRootDtoStubRepository(List<AggregateRootDtoStub> dtos)
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

        public void Add(AggregateRootDtoStub entity)
        {
            this.Dtos.Add(entity);
        }

        public void Remove(string identifier)
        {
            this.Dtos.RemoveAt(this.Dtos.FindIndex(e => e.Id == identifier));
        }

        public void Update(AggregateRootDtoStub entity)
        {
            this.Dtos[this.Dtos.FindIndex(e => e.Id == entity.Id)] = entity;
        }
    }
}