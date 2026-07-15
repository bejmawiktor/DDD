using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class ValueObject<TDeriviedValueObject, TValidator>
    : ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator>
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

public abstract class ValueObject<TValue, TDeriviedValueObject, TValidator>
    : ValueObject<TValue>,
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TValue, TDeriviedValueObject, TValidator>
{
    protected TValidator Validator
    {
        get
        {
            field.Update((TDeriviedValueObject)this);

            return field;
        }
    } = new();

    protected ValueObject(TValue value)
        : base(value) { }
}
