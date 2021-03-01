using DDD.Application;

namespace DDD.Tests.Unit.TestDoubles
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