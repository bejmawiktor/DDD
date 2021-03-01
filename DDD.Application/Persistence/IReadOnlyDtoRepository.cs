using DDD.Domain.Persistence;
using System.Collections.Generic;

namespace DDD.Application.Persistence
{
    public interface IReadOnlyDtoRepository<TDto, TDtoIdentifier>
    {
        TDto Get(TDtoIdentifier identifier);

        IEnumerable<TDto> Get(Pagination pagination = null);
    }
}