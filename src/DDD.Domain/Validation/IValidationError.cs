using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

public interface IValidationError<out TException> : IError<TException>
    where TException : Exception { }
