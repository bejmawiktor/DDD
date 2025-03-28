using DDD.Domain.Model;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation;

public abstract class DomainObjectValidator<TValidationSource, TValidationTarget>
    : TargetedValidatorBase<TValidationSource, TValidationTarget, IError>
    where TValidationSource : new()
    where TValidationTarget : IValidationTarget<TValidationTarget, TValidationSource>, IDomainObject
{
    internal void Update(TValidationTarget target) => this.UpdateSource(target);
}
