using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidationHandler<TExceptionBase>
    : ScopeHandler<
        ValidationScope<TExceptionBase>,
        TExceptionBase,
        ValidationHandler<TExceptionBase>
    >
    where TExceptionBase : Exception
{
    public override IDispatcher<TExceptionBase>? Dispatcher { get; set; }

    public ValidationHandler()
    {
        this.Dispatcher = new ValidationDispatcher<TExceptionBase>();
    }
}
