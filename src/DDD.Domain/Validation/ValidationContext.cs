using System;
using System.Collections.Generic;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

public class ValidationContext<TException>
    : DisposableScope<ValidationContext<TException>, ValidationContextHandler<TException>>
    where TException : Exception
{
    protected internal List<TException> Exceptions { get; set; }

    public Func<TException, TException> ExceptionConverter { get; }

    public ValidationContext(Func<TException, TException> exceptionConverter)
    {
        ValidateValidationExceptionConverter(exceptionConverter);

        this.Exceptions = [];
        this.ExceptionConverter = exceptionConverter;
    }

    public override void Clear() => this.Exceptions.Clear();

    private static void ValidateValidationExceptionConverter(
        Func<TException, TException> exceptionConverter
    ) => ArgumentNullException.ThrowIfNull(exceptionConverter, nameof(exceptionConverter));

    internal TException Convert(TException exception) => this.ExceptionConverter(exception);
}
