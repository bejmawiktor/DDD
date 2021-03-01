using DDD.Application.Persistence.Adapters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncReadOnlyRepositoryAdapter
    : IAsyncReadOnlyRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubRORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAsyncAggregateRootStubRORepository
    {
        AsyncAggregateRootDtoStubRORepository IAsyncReadOnlyRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubRORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository
            => this.DtoRepository;

        public AsyncAggregateRootDtoStubRORepository DtoRepository { get; }

        public AsyncReadOnlyRepositoryAdapter(AsyncAggregateRootDtoStubRORepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}