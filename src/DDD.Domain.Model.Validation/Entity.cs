using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class Entity<TIdentifier, TDeriviedEntity, TValidator>
    : Entity<TIdentifier>,
        IValidationTarget<TDeriviedEntity, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TIdentifier, TDeriviedEntity, TValidator>
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
