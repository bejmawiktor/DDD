using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Domain.Model.Extended;

public abstract class ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
    : Model.ValueObject,
        IValidationTarget<TDeriviedValueObject, TValidationSource>
    where TValidator : DomainObjectValidator<TValidationSource, TDeriviedValueObject>, new()
    where TDeriviedValueObject : ValueObject<TDeriviedValueObject, TValidator, TValidationSource>
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
}

public abstract class ValueObject<TValue, TDeriviedValueObject, TValidator, TValidationSource>
    : Model.ValueObject<TValue>,
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
