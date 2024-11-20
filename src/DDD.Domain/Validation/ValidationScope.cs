using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidationScope<TException>
    : Scope<TException, ValidationScope<TException>, ValidationHandler<TException>>
    where TException : Exception { }
