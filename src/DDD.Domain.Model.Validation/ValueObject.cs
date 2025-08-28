using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model;

public abstract class ValueObject<TDeriviedValueObject, TValidator>
    : Model.ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator>
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TDeriviedValueObject)this);

            return this.validator;
        }
    }
}

public abstract class ValueObject<TDeriviedValueObject, TValue, TValidator>
    : Model.ValueObject<TValue>,
        IValidationTarget<TDeriviedValueObject, TValidator>
    where TValidator : DomainObjectValidator<TValidator, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValue, TValidator>
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TDeriviedValueObject)this);

            return this.validator;
        }
    }

    protected ValueObject(TValue value)
        : base(value) { }
}
