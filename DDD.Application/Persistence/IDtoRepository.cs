using DDD.Domain.Persistence;
using System.Collections.Generic;

namespace DDD.Application.Persistence
{
    public interface IDtoRepository<TDto, TDtoIdentifier>
    {
        TDto Get(TDtoIdentifier identifier);

        IEnumerable<TDto> Get(Pagination pagination = null);

        void Add(TDto entity);

        void Remove(TDtoIdentifier identifier);

        void Update(TDto entity);
    }
}