﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Application.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class AsyncAggregateRootDtoStubRepository : IAsyncDtoRepository<AggregateRootDtoStub, string>
{
    public List<AggregateRootDtoStub>? Dtos { get; private set; }

    public AsyncAggregateRootDtoStubRepository(List<AggregateRootDtoStub>? dtos)
    {
        this.Dtos = dtos;
    }

    public Task<AggregateRootDtoStub?> GetAsync(string identifier) =>
        Task.FromResult(this.Dtos?.FirstOrDefault(d => d.Id == identifier));

    public Task AddAsync(AggregateRootDtoStub dto) => Task.Run(() => this.Dtos?.Add(dto));

    public Task RemoveAsync(AggregateRootDtoStub dto) =>
        Task.Run(() => this.Dtos?.RemoveAt(this.Dtos.FindIndex(e => e.Id == dto.Id)));

    public Task UpdateAsync(AggregateRootDtoStub dto) =>
        this.Dtos is null
            ? Task.Run(() => { })
            : Task.Run(() => this.Dtos[this.Dtos.FindIndex(e => e.Id == dto.Id)] = dto);
}
