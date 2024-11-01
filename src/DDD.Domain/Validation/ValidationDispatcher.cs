using System;
using System.Threading.Tasks;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidationDispatcher<TExceptionBase> : IDispatcher<TExceptionBase>
    where TExceptionBase : Exception
{
    public void Dispatch<TException>(TException exception)
        where TException : TExceptionBase => throw exception;

    public Task DispatchAsync<TException>(TException exception)
        where TException : TExceptionBase => Task.FromException(exception);
}
