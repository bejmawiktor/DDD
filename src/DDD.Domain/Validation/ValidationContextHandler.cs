using System;
using System.Threading;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

public class ValidationContextHandler<TException>
    : IScopeContainer<ValidationContext<TException>, ValidationContextHandler<TException>>
    where TException : Exception
{
    private static readonly AsyncLocal<ValidationContext<TException>?> localEventsScope = new();

    public static ValidationContext<TException>? CurrentScope
    {
        get => ValidationContextHandler<TException>.localEventsScope.Value;
        internal set => ValidationContextHandler<TException>.localEventsScope.Value = value;
    }

    static ValidationContext<TException>? IScopeContainer<
        ValidationContext<TException>,
        ValidationContextHandler<TException>
    >.CurrentScope
    {
        get => ValidationContextHandler<TException>.CurrentScope;
        set => ValidationContextHandler<TException>.CurrentScope = value;
    }

    public ValidationContextHandler() { }

    public static TException Convert(TException exception)
    {
        TException convertedException = exception;

        if (CurrentScope == null)
        {
            return convertedException;
        }

        for (
            ValidationContext<TException>? context =
                ValidationContextHandler<TException>.CurrentScope;
            context is not null;
            context = context.ParentScope
        )
        {
            convertedException = context.Convert(convertedException);
        }

        return convertedException;
    }
}
