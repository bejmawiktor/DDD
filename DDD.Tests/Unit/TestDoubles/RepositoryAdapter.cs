using DDD.Application;

namespace DDD.Tests.Unit.TestDoubles
{
    public class RepositoryAdapter
    : IRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubRepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAggregateRootStubRepository
    {
        AggregateRootDtoStubRepository IRepositoryAdapter<AggregateRootDtoStub, string, AggregateRootDtoStubRepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository 
            => this.DtoRepository;

        public AggregateRootDtoStubRepository DtoRepository { get; }

        public RepositoryAdapter(AggregateRootDtoStubRepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}