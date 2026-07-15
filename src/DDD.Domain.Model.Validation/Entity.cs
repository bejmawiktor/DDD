using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class Entity<TIdentifier, TDeriviedEntity, TValidator>(TIdentifier id)
    : Entity<TIdentifier>(id),
        IValidationTarget<TDeriviedEntity, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TIdentifier, TDeriviedEntity, TValidator>
    where TIdentifier : notnull, IEquatable<TIdentifier>
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedEntity)this);

            return field;
        }
    } = new();
}
