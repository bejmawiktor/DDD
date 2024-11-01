using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidatorHandler<TExceptionBase>
    : ScopeHandler<ValidatorScope<TExceptionBase>, TExceptionBase, ValidatorHandler<TExceptionBase>>
    where TExceptionBase : Exception
{
    public override IDispatcher<TExceptionBase>? Dispatcher { get; set; }

    public ValidatorHandler()
    {
        this.Dispatcher = new ValidatorDispatcher<TExceptionBase>();
    }
}
