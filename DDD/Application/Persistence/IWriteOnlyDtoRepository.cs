namespace DDD.Application
{
    public interface IWriteOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        void Add(TDto entity);

        void Remove(TDto entity);

        void Update(TDto entity);
    }
}