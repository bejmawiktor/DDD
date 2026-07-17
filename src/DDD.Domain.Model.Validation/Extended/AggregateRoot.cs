using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

/// <summary>
/// Extended aggregate root that separates the validator type from its
/// validation source, allowing the state fed to validation to differ from the
/// validator itself. Accessing <see cref="Validator"/> refreshes it with the
/// current state before validation is performed.
/// </summary>
/// <typeparam name="TIdentifier">Type of the aggregate root identifier.</typeparam>
/// <typeparam name="TDeriviedAggregateRoot">
/// The concrete aggregate root type deriving from this class (curiously
/// recurring generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this aggregate root. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <typeparam name="TValidationSource">
/// The state type the validator inspects. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <param name="id">The identifier of the aggregate root. Cannot be <see langword="null"/>.</param>
public abstract class AggregateRoot<
    TIdentifier,
    TDeriviedAggregateRoot,
    TValidator,
    TValidationSource
>(TIdentifier id)
    : AggregateRoot<TIdentifier>(id),
        IValidationTarget<TDeriviedAggregateRoot, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<
            TIdentifier,
            TDeriviedAggregateRoot,
            TValidator,
            TValidationSource
        >
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the validator for this aggregate root, refreshed with the current
    /// instance state on every access.
    /// </summary>
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedAggregateRoot)this);

            return field;
        }
    } = new();
}
