using DDD.Application.Persistence;
using DDD.Domain.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncAggregateRootDtoStubRepository : IAsyncDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub> Dtos { get; private set; }

        public AsyncAggregateRootDtoStubRepository(List<AggregateRootDtoStub> dtos)
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