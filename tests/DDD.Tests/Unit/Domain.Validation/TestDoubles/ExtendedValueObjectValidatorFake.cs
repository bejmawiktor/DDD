using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class ValueObjectValidationSource
{
    public string? TextField { get; set; }
    public int? IntField { get; set; }
}

internal class ExtendedValueObjectValidatorFake
    : DomainObjectValidator<ValueObjectValidationSource, ExtendedValidatedValueObjectFake>
{
    public static readonly string EmptyTextFieldErrorMessage =
        "Empty TextField given. This field is required.";
    public static readonly string LessThanZeroIntFieldErrorMessage =
        "IntField must be greater than zero.";
    public static readonly string GreaterThanTenIntFieldErrorMessage =
        "IntField must less than 10.";

    public static readonly string MinusFiveIntFieldErrorMessage = "Forbidden value occured.";

    protected override ValueObjectValidationSource ValidationSource { get; }

    public ExtendedValueObjectValidatorFake()
        : base()
    {
        this.ValidationSource = new ValueObjectValidationSource();
        _ = this
            .Configuration.WithValidation(
                nameof(ValueObjectValidationSource.TextField),
                value =>
                    value.TextField == string.Empty
                        ? new ValidationError(
                            nameof(ValueObjectValidationSource.TextField),
                            EmptyTextFieldErrorMessage
                        )
                        : null
            )
            .WithValidation(
                nameof(ValueObjectValidationSource.IntField),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.IntField < 0
                                ? new ValidationError(
                                    nameof(ValueObjectValidationSource.IntField),
                                    LessThanZeroIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField > 10
                                ? new ValidationError(
                                    nameof(ValueObjectValidationSource.IntField),
                                    GreaterThanTenIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField == -5
                                ? new ValidationError(
                                    nameof(ValueObjectValidationSource.IntField),
                                    MinusFiveIntFieldErrorMessage
                                )
                                : null
                        )
            );
    }

    protected override void UpdateSource(ExtendedValidatedValueObjectFake validationTarget)
    {
        this.ValidationSource.TextField = validationTarget.TextField;
        this.ValidationSource.IntField = validationTarget.IntField;
    }
}
