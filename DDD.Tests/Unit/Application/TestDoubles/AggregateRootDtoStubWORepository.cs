using DDD.Application.Persistence;
using System.Collections.Generic;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStubWORepository : IWriteOnlyDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AggregateRootDtoStubWORepository(List<AggregateRootDtoStub> dtos)
        {
            this.Dtos = dtos;
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