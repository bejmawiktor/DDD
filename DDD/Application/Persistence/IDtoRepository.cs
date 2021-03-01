namespace DDD.Application
{
    public interface IDtoRepository<TDto, TDtoIdentifier> : IReadOnlyDtoRepository<TDto, TDtoIdentifier>, IWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
    }
}