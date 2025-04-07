using Utils.Validation;

namespace DDD.Domain.Validation;

public abstract class ValueObject<TValueObject, TValidator, TValidationSource>
    : Model.ValueObject,
        IValidationTarget<TValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TValueObject>, new()
    where TValueObject : ValueObject<TValueObject, TValidator, TValidationSource>
    where TValidationSource : new()
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TValueObject)this);

            return this.validator;
        }
    }
}

public abstract class ValueObject<TValueObject, TValue, TValidator, TValidationSource>
    : Model.ValueObject<TValue>,
        IValidationTarget<TValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TValueObject>, new()
    where TValueObject : ValueObject<TValueObject, TValue, TValidator, TValidationSource>
    where TValidationSource : new()
{
    private readonly TValidator validator = new();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TValueObject)this);

            return this.validator;
        }
    }

    protected ValueObject(TValue value)
        : base(value) { }
}
