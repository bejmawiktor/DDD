using DDD.Domain.Model;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation;

public class AggregateRoot<TAggregateRoot, TIdentifier, TValidator, TValidationSource>
    : AggregateRoot<TIdentifier>,
        IValidationTarget<TAggregateRoot, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TAggregateRoot>, new()
    where TAggregateRoot : AggregateRoot<TAggregateRoot, TIdentifier, TValidator, TValidationSource>
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    protected AggregateRoot(TIdentifier id)
        : base(id) { }

    protected ValidationResult<IError> Validate(Action<TValidationSource> updateAction)
    {
        TValidator validator = new();
        validator.Update((TAggregateRoot)this);

        return validator.Validate(updateAction);
    }

    protected ValidationResult<IError> Validate(
        string validatorName,
        Action<TValidationSource> updateAction
    )
    {
        TValidator validator = new();
        validator.Update((TAggregateRoot)this);

        return validator.Validate(validatorName, updateAction);
    }
}
