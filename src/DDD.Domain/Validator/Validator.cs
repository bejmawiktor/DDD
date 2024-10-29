using System;

namespace DDD.Domain.Validator;

public static class Validator
{
    public static void Throw<TException>(TException exception)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);

        ValidatorHandler.Instance.Handle(exception);
    }

    public static ValidationResult<TResult> ValidateMany<TResult>(Func<TResult> validationFunc)
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidatorScope scope = new();

        TResult result = validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TResult>(scope.Items.ToArray())
            : new ValidationResult<TResult>(result);
    }
}
