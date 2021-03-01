using DDD.Application.Persistence.Adapters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class ReadOnlyRepositoryAdapter
    : IReadOnlyRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubRORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAggregateRootStubRORepository
    {
        AggregateRootDtoStubRORepository IReadOnlyRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubRORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository
            => this.DtoRepository;

        public AggregateRootDtoStubRORepository DtoRepository { get; }

        public ReadOnlyRepositoryAdapter(AggregateRootDtoStubRORepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}