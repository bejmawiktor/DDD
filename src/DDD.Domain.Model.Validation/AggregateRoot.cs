using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

/// <summary>
/// Aggregate root that carries a dedicated <see cref="DomainObjectValidator{TValidationSource, TValidationTarget}"/>
/// for enforcing its invariants. Accessing <see cref="Validator"/> refreshes it
/// with the current state before validation is performed.
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
/// <param name="id">The identifier of the aggregate root. Cannot be <see langword="null"/>.</param>
public abstract class AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>(TIdentifier id)
    : AggregateRoot<TIdentifier>(id),
        IValidationTarget<TDeriviedAggregateRoot, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedAggregateRoot>, new()
    where TDeriviedAggregateRoot : AggregateRoot<TIdentifier, TDeriviedAggregateRoot, TValidator>
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
