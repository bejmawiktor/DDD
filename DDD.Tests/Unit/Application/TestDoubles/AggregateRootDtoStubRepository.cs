using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Linq;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AggregateRootDtoStubRepository : IDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub>? Dtos { get; private set; }

        public AggregateRootDtoStubRepository(List<AggregateRootDtoStub>? dtos)
        {
            this.Dtos = dtos;
        }

        public AggregateRootDtoStub? Get(string identifier)
        {
            return this.Dtos?.FirstOrDefault(d => d.Id == identifier);
        }

        public IEnumerable<AggregateRootDtoStub> Get(Pagination? pagination = null)
        {
            return this.Dtos ?? Enumerable.Empty<AggregateRootDtoStub>();
        }

        public void Add(AggregateRootDtoStub dto)
        {
            this.Dtos?.Add(dto);
        }

        public void Remove(AggregateRootDtoStub dto)
        {
            this.Dtos?.RemoveAt(this.Dtos.FindIndex(e => e.Id == dto.Id));
        }

        public void Update(AggregateRootDtoStub dto)
        {
            if(this.Dtos is not null)
            {
                this.Dtos[this.Dtos.FindIndex(e => e.Id == dto.Id)] = dto;
            }
        }
    }
}