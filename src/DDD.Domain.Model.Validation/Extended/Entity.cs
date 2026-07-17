using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

/// <summary>
/// Extended entity that separates the validator type from its validation
/// source, allowing the state fed to validation to differ from the validator
/// itself. Accessing <see cref="Validator"/> refreshes it with the current state
/// before validation is performed.
/// </summary>
/// <typeparam name="TIdentifier">Type of the entity identifier.</typeparam>
/// <typeparam name="TDeriviedEntity">
/// The concrete entity type deriving from this class (curiously recurring
/// generic pattern).
/// </typeparam>
/// <typeparam name="TValidator">
/// The validator type for this entity. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <typeparam name="TValidationSource">
/// The state type the validator inspects. Must have a public parameterless
/// constructor.
/// </typeparam>
/// <param name="id">The identifier of the entity. Cannot be <see langword="null"/>.</param>
public abstract class Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>(
    TIdentifier id
) : Entity<TIdentifier>(id), IValidationTarget<TDeriviedEntity, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the validator for this entity, refreshed with the current instance
    /// state on every access.
    /// </summary>
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedEntity)this);

            return field;
        }
    } = new();
}
