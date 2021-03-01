namespace DDD.Application
{
    public interface IAsyncDtoRepository<TDto, TDtoIdentifier> : IAsyncReadOnlyDtoRepository<TDto, TDtoIdentifier>, IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
    }
}