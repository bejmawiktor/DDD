using System.Collections.Generic;
using DDD.Domain.Persistence;

namespace DDD.Application.Persistence
{
    public interface IDtoRepository<TDto, TDtoIdentifier>
    {
        TDto? Get(TDtoIdentifier identifier);

        IEnumerable<TDto> Get(Pagination? pagination = null);

        void Add(TDto dto);

        void Remove(TDto dto);

        void Update(TDto dto);
    }
}
