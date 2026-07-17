using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

/// <summary>
/// Value object that carries a dedicated
/// <see cref="DomainObjectValidator{TValidationSource, TValidationTarget}"/> for
/// enforcing its invariants. Accessing <see cref="Validator"/> refreshes it with
/// the current state before validation is performed.
/// </summary>
/// <typeparam name="TDeriviedValueObject">
/// The concrete value object type deriving from this class (curiously recurring
/// generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this value object. Must have a public parameterless
/// constructor.
/// </typeparam>
public abstract class ValueObject<TDeriviedValueObject, TValidator>
    : ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator>
{
    /// <summary>
    /// Gets the validator for this value object, refreshed with the current
    /// instance state on every access.
    /// </summary>
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedValueObject)this);

            return field;
        }
    } = new();
}

/// <summary>
/// Single-value value object that wraps and validates one underlying value and
/// carries a dedicated
/// <see cref="DomainObjectValidator{TValidationSource, TValidationTarget}"/>.
/// Accessing <see cref="Validator"/> refreshes it with the current state before
/// validation is performed.
/// </summary>
/// <typeparam name="TValue">Type of the wrapped value.</typeparam>
/// <typeparam name="TDeriviedValueObject">
/// The concrete value object type deriving from this class (curiously recurring
/// generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this value object. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <param name="value">The value to wrap.</param>
public abstract class ValueObject<TValue, TDeriviedValueObject, TValidator>(TValue value)
    : ValueObject<TValue>(value),
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TValue, TDeriviedValueObject, TValidator>
{
    /// <summary>
    /// Gets the validator for this value object, refreshed with the current
    /// instance state on every access.
    /// </summary>
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedValueObject)this);

            return field;
        }
    } = new();
}
