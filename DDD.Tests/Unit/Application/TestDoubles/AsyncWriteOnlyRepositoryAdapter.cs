using DDD.Application.Persistence.Adapters;

namespace DDD.Tests.Unit.Application.TestDoubles
{
    public class AsyncWriteOnlyRepositoryAdapter
        : IAsyncWriteOnlyRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubWORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>,
        IAsyncAggregateRootStubWORepository
    {
        public AsyncAggregateRootDtoStubWORepository DtoRepository { get; }

        AsyncAggregateRootDtoStubWORepository IAsyncWriteOnlyRepositoryAdapter<AggregateRootDtoStub, string, AsyncAggregateRootDtoStubWORepository, AggregateRootDtoStubConverter, AggregateRootStub, string>.DtoRepository
            => this.DtoRepository;

        public AsyncWriteOnlyRepositoryAdapter(AsyncAggregateRootDtoStubWORepository dtoRepository)
            => this.DtoRepository = dtoRepository;
    }
}