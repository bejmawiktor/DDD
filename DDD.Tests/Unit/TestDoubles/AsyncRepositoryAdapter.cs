using DDD.Application;

namespace DDD.Tests.Unit.TestDoubles
{
    public class AsyncRepositoryAdapter
    : IAsyncRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubRepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAsyncAggregateRootStubRepository
    {
        AsyncAggregateRootDtoStubRepository IAsyncRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubRepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository
            => this.DtoRepository;

        public AsyncAggregateRootDtoStubRepository DtoRepository { get; }

        public AsyncRepositoryAdapter(AsyncAggregateRootDtoStubRepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}