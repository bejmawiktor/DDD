using System;
using System.Threading.Tasks;

namespace DDD.Domain.Validation;

public static class Validator<TExceptionBase>
    where TExceptionBase : Exception
{
    public static void Throw<TException>(TException exception)
        where TException : TExceptionBase
    {
        ArgumentNullException.ThrowIfNull(exception);

        ValidationHandler<TExceptionBase>.Instance.Handle(exception);
    }

    public static ValidationResult<TExceptionBase> TryMany(Action validationAction)
    {
        ArgumentNullException.ThrowIfNull(validationAction);

        using ValidationScope<TExceptionBase> scope = new();

        validationAction();

        return scope.Items.Count > 0
            ? new ValidationResult<TExceptionBase>([.. scope.Items])
            : new ValidationResult<TExceptionBase>();
    }

    public static ValidationResult<TResult, TExceptionBase> TryMany<TResult>(
        Func<TResult> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidationScope<TExceptionBase> scope = new();

        TResult result = validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TResult, TExceptionBase>([.. scope.Items])
            : new ValidationResult<TResult, TExceptionBase>(result);
    }

    public static async Task<ValidationResult<TExceptionBase>> TryManyAsync(
        Func<Task> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidationScope<TExceptionBase> scope = new();

        await validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TExceptionBase>([.. scope.Items])
            : new ValidationResult<TExceptionBase>();
    }

    public static async Task<ValidationResult<TResult, TExceptionBase>> TryManyAsync<TResult>(
        Func<Task<TResult>> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidationScope<TExceptionBase> scope = new();

        TResult result = await validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TResult, TExceptionBase>([.. scope.Items])
            : new ValidationResult<TResult, TExceptionBase>(result);
    }
}
