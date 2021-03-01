using DDD.Application.Persistence.Adapters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class WriteOnlyRepositoryAdapter
        : IWriteOnlyRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubWORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAggregateRootStubWORepository
    {
        public AggregateRootDtoStubWORepository DtoRepository { get; }

        AggregateRootDtoStubWORepository IWriteOnlyRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubWORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository
            => this.DtoRepository;

        public WriteOnlyRepositoryAdapter(AggregateRootDtoStubWORepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}