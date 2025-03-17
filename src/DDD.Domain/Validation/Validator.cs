using System;
using System.Threading.Tasks;

namespace DDD.Domain.Validation;

public class Validator<TExceptionBase>
    where TExceptionBase : Exception
{
    protected Validator() { }

    public static void Throw<TException>(TException exception)
        where TException : TExceptionBase
    {
        ArgumentNullException.ThrowIfNull(exception);

        TException convertedException = ValidationContextHandler<TException>.Convert(exception);

        ValidationHandler<TExceptionBase>.Instance.Handle(convertedException);
    }

    public static ValidationResult<TExceptionBase> ValidateMany(Action validationAction)
    {
        ArgumentNullException.ThrowIfNull(validationAction);

        using ValidationScope<TExceptionBase> scope = new();

        validationAction();

        return scope.Items.Count > 0
            ? new ValidationResult<TExceptionBase>([.. scope.Items])
            : new ValidationResult<TExceptionBase>();
    }

    public static ValidationResult<TValue, TExceptionBase> ValidateMany<TValue>(
        Func<TValue> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidationScope<TExceptionBase> scope = new();

        TValue value = validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TValue, TExceptionBase>([.. scope.Items])
            : new ValidationResult<TValue, TExceptionBase>(value);
    }

    public static async Task<ValidationResult<TExceptionBase>> ValidateManyAsync(
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

    public static async Task<ValidationResult<TValue, TExceptionBase>> ValidateManyAsync<TValue>(
        Func<Task<TValue>> validationFunc
    )
    {
        ArgumentNullException.ThrowIfNull(validationFunc);

        using ValidationScope<TExceptionBase> scope = new();

        TValue value = await validationFunc();

        return scope.Items.Count > 0
            ? new ValidationResult<TValue, TExceptionBase>([.. scope.Items])
            : new ValidationResult<TValue, TExceptionBase>(value);
    }
}
