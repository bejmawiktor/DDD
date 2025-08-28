using DDD.Domain.Model;
using Utils.Validation;

namespace DDD.Domain.Validation;

public abstract class Entity<TDeriviedEntity, TIdentifier, TValidator, TValidationSource>
    : Entity<TIdentifier>,
        IValidationTarget<TDeriviedEntity, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TDeriviedEntity, TIdentifier, TValidator, TValidationSource>
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
