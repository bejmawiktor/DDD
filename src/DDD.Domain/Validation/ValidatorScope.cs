using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidatorScope<TException>
    : Scope<TException, ValidatorScope<TException>, ValidatorHandler<TException>>
    where TException : Exception { }
