using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

/// <summary>
/// Extended value object that separates the validator type from its validation
/// source, allowing the state fed to validation to differ from the validator
/// itself. Accessing <see cref="Validator"/> refreshes it with the current state
/// before validation is performed.
/// </summary>
/// <typeparam name="TDeriviedValueObject">
/// The concrete value object type deriving from this class (curiously recurring
/// generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this value object. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <typeparam name="TValidationSource">
/// The state type the validator inspects. Must have a public parameterless
/// constructor.
/// </typeparam>
public abstract class ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
    : ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
    where TValidationSource : new()
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
/// Extended single-value value object that wraps and validates one underlying
/// value while separating the validator type from its validation source.
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
/// <typeparam name="TValidationSource">
/// The state type the validator inspects. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <param name="value">The value to wrap.</param>
public abstract class ValueObject<TValue, TDeriviedValueObject, TValidator, TValidationSource>(
    TValue value
) : ValueObject<TValue>(value), IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<
            TValue,
            TDeriviedValueObject,
            TValidator,
            TValidationSource
        >
    where TValidationSource : new()
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
