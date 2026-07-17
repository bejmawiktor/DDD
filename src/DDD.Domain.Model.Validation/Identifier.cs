using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

/// <summary>
/// Strongly typed identifier that carries a dedicated
/// <see cref="DomainObjectValidator{TValidationSource, TValidationTarget}"/> for
/// enforcing its invariants. Accessing <see cref="Validator"/> refreshes it with
/// the current state before validation is performed.
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
/// <param name="value">The underlying identifier value. Cannot be <see langword="null"/>.</param>
public abstract class Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>(TIdentifierValue value)
    : Identifier<TIdentifierValue, TDeriviedIdentifier>(value),
        IValidationTarget<TDeriviedIdentifier, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedIdentifier>, new()
    where TDeriviedIdentifier : Identifier<TIdentifierValue, TDeriviedIdentifier, TValidator>
    where TIdentifierValue : notnull, IEquatable<TIdentifierValue>
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
