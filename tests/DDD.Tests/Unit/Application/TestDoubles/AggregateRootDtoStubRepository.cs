using DDD.Application.Persistence;

namespace DDD.Tests.Unit.Application.TestDoubles;

public class AggregateRootDtoStubRepository(List<AggregateRootDtoStub>? dtos)
    : IDtoRepository<AggregateRootDtoStub, string>
{
    public List<AggregateRootDtoStub>? Dtos { get; private set; } = dtos;

    public AggregateRootDtoStub? Get(string identifier) =>
        this.Dtos?.FirstOrDefault(d => d.Id == identifier);

    public void Add(AggregateRootDtoStub dto) => this.Dtos?.Add(dto);

    public void Remove(AggregateRootDtoStub dto) =>
        this.Dtos?.RemoveAt(this.Dtos.FindIndex(e => e.Id == dto.Id));

    public void Update(AggregateRootDtoStub dto) =>
        this.Dtos?[this.Dtos.FindIndex(e => e.Id == dto.Id)] = dto;
}
