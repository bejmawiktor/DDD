namespace DDD.Application.Persistence
{
    public interface IAsyncDtoRepository<TDto, TDtoIdentifier> : IAsyncReadOnlyDtoRepository<TDto, TDtoIdentifier>, IAsyncWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
    }
}