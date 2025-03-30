using Utils.Functional;
using Utils.Validation;

namespace DDD.Domain.Validation;

public abstract class ValueObject<TValueObject, TValidator, TValidationSource>
    : Model.ValueObject,
        IValidationTarget<TValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TValueObject>, new()
    where TValueObject : ValueObject<TValueObject, TValidator, TValidationSource>
    where TValidationSource : new()
{
    private TValidator validator = new TValidator();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TValueObject)this);

            return validator;
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
    private TValidator validator = new TValidator();

    protected TValidator Validator
    {
        get
        {
            this.validator.Update((TValueObject)this);

            return validator;
        }
    }

    protected ValueObject(TValue value)
        : base(value) { }
}
