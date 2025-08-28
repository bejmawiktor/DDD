using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>
    : Entity<TIdentifier>,
        IValidationTarget<TDeriviedEntity, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>
    where TValidationSource : new()
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TDeriviedEntity)this);

            return this.validator;
        }
    }

    protected Entity(TIdentifier id)
        : base(id) { }
}
