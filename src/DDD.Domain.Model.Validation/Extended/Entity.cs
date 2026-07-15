using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>(TIdentifier id)
    : Entity<TIdentifier>(id),
        IValidationTarget<TDeriviedEntity, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedEntity>, new()
    where TDeriviedEntity : Entity<TIdentifier, TDeriviedEntity, TValidator, TValidationSource>
    where TValidationSource : new()
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
