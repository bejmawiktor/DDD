using DDD.Domain.Model.Validation;
using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using Utils.Validation;

internal class AggregateRootValidationSource
{
    public string? TextField { get; set; }
    public int? IntField { get; set; }
}

internal class ExtendedAggregateRootValidatorFake
    : DomainObjectValidator<AggregateRootValidationSource, ExtendedValidatedAggregateRootFake>
{
    public static readonly string EmptyTextFieldErrorMessage =
        "Empty TextField given. This field is required.";
    public static readonly string LessThanZeroIntFieldErrorMessage =
        "IntField must be greater than zero.";
    public static readonly string GreaterThanTenIntFieldErrorMessage =
        "IntField must less than 10.";

    public static readonly string MinusFiveIntFieldErrorMessage = "Forbidden value occured.";

    protected override AggregateRootValidationSource ValidationSource { get; }

    public ExtendedAggregateRootValidatorFake()
        : base()
    {
        this.ValidationSource = new AggregateRootValidationSource();

        _ = this
            .Configuration.WithValidation(
                nameof(AggregateRootValidationSource.TextField),
                value =>
                    value.TextField == string.Empty
                        ? new ValidationError(
                            nameof(AggregateRootValidationSource.TextField),
                            EmptyTextFieldErrorMessage
                        )
                        : null
            )
            .WithValidation(
                nameof(AggregateRootValidationSource.IntField),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.IntField < 0
                                ? new ValidationError(
                                    nameof(AggregateRootValidationSource.IntField),
                                    LessThanZeroIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField > 10
                                ? new ValidationError(
                                    nameof(AggregateRootValidationSource.IntField),
                                    GreaterThanTenIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField == -5
                                ? new ValidationError(
                                    nameof(AggregateRootValidationSource.IntField),
                                    MinusFiveIntFieldErrorMessage
                                )
                                : null
                        )
            );
    }

    protected override void UpdateSource(ExtendedValidatedAggregateRootFake validationTarget)
    {
        this.ValidationSource.TextField = validationTarget.TextField;
        this.ValidationSource.IntField = validationTarget.IntField;
    }
}
