using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

/// <summary>
/// Extended strongly typed identifier that separates the validator type from its
/// validation source, allowing the state fed to validation to differ from the
/// validator itself. Accessing <see cref="Validator"/> refreshes it with the
/// current state before validation is performed.
/// </summary>
/// <typeparam name="TIdentifierValue">Type of the underlying identifier value.</typeparam>
/// <typeparam name="TDeriviedIdentifier">
/// The concrete identifier type deriving from this class (curiously recurring
/// generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this identifier. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <typeparam name="TValidationSource">
/// The state type the validator inspects. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <param name="value">The underlying identifier value. Cannot be <see langword="null"/>.</param>
public abstract class Identifier<
    TIdentifierValue,
    TDeriviedIdentifier,
    TValidator,
    TValidationSource
>(TIdentifierValue value)
    : Identifier<TIdentifierValue, TDeriviedIdentifier>(value),
        IValidationTarget<TDeriviedIdentifier, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<
            TIdentifierValue,
            TDeriviedIdentifier,
            TValidator,
            TValidationSource
        >
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
    where TValidationSource : new()
{
    /// <summary>
    /// Gets the validator for this identifier, refreshed with the current
    /// instance state on every access.
    /// </summary>
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedIdentifier)this);

            return field;
        }
    } = new();
}
