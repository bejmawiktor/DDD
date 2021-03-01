using DDD.Application;
using DDD.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Tests.Unit.TestDoubles
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

        public void Remove(AggregateRootDtoStub entity)
        {
            this.Dtos.RemoveAt(this.Dtos.FindIndex(e => e.Id == entity.Id));
        }

        public void Update(AggregateRootDtoStub entity)
        {
            this.Dtos[this.Dtos.FindIndex(e => e.Id == entity.Id)] = entity;
        }
    }
}