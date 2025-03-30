using DDD.Domain.Model;
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
    private TValidator validator = new TValidator();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TEntity)this);

            return validator;
        }
    }

    protected Entity(TIdentifier id)
        : base(id) { }
}
