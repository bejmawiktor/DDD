using DDD.Application.Persistence.Adapters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class RepositoryAdapter
        : IRepositoryAdapter<
            AggregateRootDtoStub,
            string,
            AggregateRootDtoStubRepository,
            AggregateRootDtoStubConverter,
            AggregateRootStub,
            string
        >,
            IAggregateRootStubRepository
    {
        AggregateRootDtoStubRepository IRepositoryAdapter<
            AggregateRootDtoStub,
            string,
            AggregateRootDtoStubRepository,
            AggregateRootDtoStubConverter,
            AggregateRootStub,
            string
        >.DtoRepository => this.DtoRepository;

        public AggregateRootDtoStubRepository DtoRepository { get; }

        public RepositoryAdapter(AggregateRootDtoStubRepository dtoRepository) =>
            this.DtoRepository = dtoRepository;
    }
}
