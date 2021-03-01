using DDD.Application;

namespace DDD.Tests.Unit.TestDoubles
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