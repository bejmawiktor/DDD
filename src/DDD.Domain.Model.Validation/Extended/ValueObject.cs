using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
    : ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
    where TValidationSource : new()
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedValueObject)this);

            return field;
        }
    } = new();
}

public abstract class ValueObject<TValue, TDeriviedValueObject, TValidator, TValidationSource>(TValue value)
    : ValueObject<TValue>(value),
        IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<
            TValue,
            TDeriviedValueObject,
            TValidator,
            TValidationSource
        >
    where TValidationSource : new()
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedValueObject)this);

            return field;
        }
    } = new();
}
