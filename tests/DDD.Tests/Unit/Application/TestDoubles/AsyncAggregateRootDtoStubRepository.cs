using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Persistence;
using DDD.Domain.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncAggregateRootDtoStubRepository
        : IAsyncDtoRepository<AggregateRootDtoStub, string>
    {
        public List<AggregateRootDtoStub>? Dtos { get; private set; }

        public AsyncAggregateRootDtoStubRepository(List<AggregateRootDtoStub>? dtos)
        {
            this.Dtos = dtos;
        }

        public Task<AggregateRootDtoStub?> GetAsync(string identifier)
        {
            return Task.FromResult(this.Dtos?.FirstOrDefault(d => d.Id == identifier));
        }

        public Task<IEnumerable<AggregateRootDtoStub>> GetAsync(Pagination? pagination = null)
        {
            return Task.FromResult(
                this.Dtos?.AsEnumerable() ?? Enumerable.Empty<AggregateRootDtoStub>()
            );
        }

        public Task AddAsync(AggregateRootDtoStub dto)
        {
            return Task.Run(() => this.Dtos?.Add(dto));
        }

        public Task RemoveAsync(AggregateRootDtoStub dto)
        {
            return Task.Run(() => this.Dtos?.RemoveAt(this.Dtos.FindIndex(e => e.Id == dto.Id)));
        }

        public Task UpdateAsync(AggregateRootDtoStub dto)
        {
            if (this.Dtos is null)
            {
                return Task.Run(() => { });
            }

            return Task.Run(() => this.Dtos[this.Dtos.FindIndex(e => e.Id == dto.Id)] = dto);
        }
    }
}
