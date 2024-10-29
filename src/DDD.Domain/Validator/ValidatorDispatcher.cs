using System;
using System.Threading.Tasks;
using DDD.Domain.Utils;

namespace DDD.Domain.Validator;

internal sealed class ValidatorDispatcher : IDispatcher<Exception>
{
    public void Dispatch<TException>(TException exception)
        where TException : Exception => throw exception;

    public Task DispatchAsync<TException>(TException exception)
        where TException : Exception => Task.FromException(exception);
}
