using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Model.Validation;

/// <summary>
/// Base class for validators that check the invariants of a domain object and
/// report violations as <see cref="IError"/> values. Built on the Utils
/// <see cref="TargetedValidatorBase{TValidationSource, TValidationTarget, TError}"/>;
/// the validated object keeps the validator in sync via <see cref="Update"/>.
/// </summary>
/// <typeparam name="TValidationSource">
/// The validator's own type (curiously recurring generic pattern). Must have a
/// public parameterless constructor.
/// </typeparam>
/// <typeparam name="TValidationTarget">
/// The domain object being validated. Must be an <see cref="IDomainObject"/>
/// that declares this validator as its validation source.
/// </typeparam>
public abstract class DomainObjectValidator<TValidationSource, TValidationTarget>
    : TargetedValidatorBase<TValidationSource, TValidationTarget, IError>
    where TValidationSource : new()
    where TValidationTarget : IValidationTarget<TValidationTarget, TValidationSource>, IDomainObject
{
    /// <summary>
    /// Points the validator at the domain object instance to validate. Called by
    /// the domain object whenever its validator is accessed.
    /// </summary>
    /// <param name="target">The domain object to validate.</param>
    internal void Update(TValidationTarget target) => this.UpdateSource(target);
}
