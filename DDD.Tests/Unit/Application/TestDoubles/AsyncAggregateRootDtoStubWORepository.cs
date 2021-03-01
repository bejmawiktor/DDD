using DDD.Application.Persistence;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncAggregateRootDtoStubWORepository : IAsyncWriteOnlyDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AsyncAggregateRootDtoStubWORepository(List<AggregateRootDtoStub> dtos)
        {
            this.Dtos = dtos;
        }

        public Task AddAsync(AggregateRootDtoStub entity)
        {
            return Task.Run(() => this.Dtos.Add(entity));
        }

        public Task RemoveAsync(AggregateRootDtoStub entity)
        {
            return Task.Run(() => this.Dtos.RemoveAt(this.Dtos.FindIndex(e => e.Id == entity.Id)));
        }

        public Task UpdateAsync(AggregateRootDtoStub entity)
        {
            return Task.Run(() => this.Dtos[this.Dtos.FindIndex(e => e.Id == entity.Id)] = entity);
        }
    }
}