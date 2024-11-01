using System;
using System.Threading.Tasks;

namespace DDD.Domain.Validation;

public static class Validator
{
    public static void Throw<TException>(TException exception)
        where TException : Exception
    {
        ArgumentNullException.ThrowIfNull(exception);

        ValidatorHandler.Instance.Handle(exception);
    }

    public static ValidationResult TryMany(Action validationAction)
    {
        ArgumentNullException.ThrowIfNull(validationAction);

        using ValidatorScope scope = new();

        validationAction();

        return scope.Items.Count > 0
            ? new ValidationResult([.. scope.Items])
            : new ValidationResult();
    }

    public static ValidationResult<TResult> TryMany<TResult>(Func<TResult> validationFunc)
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidatorScope scope = new();

        TResult result = validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TResult>([.. scope.Items])
            : new ValidationResult<TResult>(result);
    }

    public static async Task<ValidationResult> TryManyAsync(Func<Task> validationFunc)
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidatorScope scope = new();

        await validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult([.. scope.Items])
            : new ValidationResult();
    }

    public static async Task<ValidationResult<TResult>> TryManyAsync<TResult>(
        Func<Task<TResult>> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidatorScope scope = new();

        TResult result = await validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TResult>([.. scope.Items])
            : new ValidationResult<TResult>(result);
    }
}
