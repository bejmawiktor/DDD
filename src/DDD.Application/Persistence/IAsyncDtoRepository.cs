﻿using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Persistence;

namespace DDD.Application.Persistence;

public interface IAsyncDtoRepository<TDto, TDtoIdentifier>
{
    Task<TDto?> GetAsync(TDtoIdentifier identifier);

    Task<IEnumerable<TDto>> GetAsync(Pagination? pagination = null);

    Task AddAsync(TDto dto);

    Task RemoveAsync(TDto dto);

    Task UpdateAsync(TDto dto);
}
