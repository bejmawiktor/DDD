﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DDD.Domain.Model;

namespace DDD.Domain.Persistence;

public interface IAsyncRepository<TAggregateRoot, TIdentifier>
    where TAggregateRoot : IAggregateRoot<TIdentifier>
    where TIdentifier : IEquatable<TIdentifier>
{
    Task<TAggregateRoot?> GetAsync(TIdentifier identifier);

    Task<IEnumerable<TAggregateRoot>> GetAsync(Pagination? pagination = null);

    Task AddAsync(TAggregateRoot entity);

    Task UpdateAsync(TAggregateRoot entity);

    Task RemoveAsync(TAggregateRoot entity);
}
