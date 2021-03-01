using DDD.Application;

namespace DDD.Tests.Unit.TestDoubles
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