using System;
using DDD.Domain.Utils;

namespace DDD.Domain.Validation;

internal sealed class ValidatorHandler : ScopeHandler<ValidatorScope, Exception, ValidatorHandler>
{
    public override IDispatcher<Exception>? Dispatcher { get; set; }

    public ValidatorHandler()
    {
        this.Dispatcher = new ValidatorDispatcher();
    }
}
