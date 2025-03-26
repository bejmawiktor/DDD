using DDD.Domain.Model;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation;

public abstract class Entity<TEntity, TIdentifier, TValidator, TValidationSource>
    : Entity<TIdentifier>,
        IValidationTarget<TEntity, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TEntity>, new()
    where TEntity : Entity<TEntity, TIdentifier, TValidator, TValidationSource>
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    protected Entity(TIdentifier id)
        : base(id) { }

    protected ValidationResult<IError> Validate(Action<TValidationSource> updateAction)
    {
        TValidator validator = new();
        validator.Update((TEntity)this);

        return validator.Validate(updateAction);
    }

    protected ValidationResult<IError> Validate(
        string validatorName,
        Action<TValidationSource> updateAction
    )
    {
        TValidator validator = new();
        validator.Update((TEntity)this);

        return validator.Validate(validatorName, updateAction);
    }
}
