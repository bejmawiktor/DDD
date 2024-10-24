using System;

namespace DDD.Domain.Persistence;

public interface ITransactionScope : IDisposable
{
    void Complete();
}