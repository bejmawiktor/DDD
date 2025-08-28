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

public abstract class ValueObject<TDeriviedValueObject, TValue, TValidator, TValidationSource>
    : Model.ValueObject<TValue>,
        IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<
            TDeriviedValueObject,
            TValue,
            TValidator,
            TValidationSource
        >
    where TValidationSource : new()
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
