namespace DDD.Application.Persistence
{
    public interface IDtoRepository<TDto, TDtoIdentifier> : IReadOnlyDtoRepository<TDto, TDtoIdentifier>, IWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
    }
}