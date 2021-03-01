using DDD.Persistence;
using System.Collections.Generic;

namespace DDD.Application
{
    public interface IReadOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        TDto Get(TDtoIdentifier identifier);

        IEnumerable<TDto> Get(Pagination pagination = null);
    }
}