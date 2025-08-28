using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

public class SingleValueValueObjectValidationSource
{
    public int? Value { get; set; }
    public int? NextValue { get; set; }
}

public class ExtendedSingleValueValueObjectValidatorFake
    : DomainObjectValidator<
        SingleValueValueObjectValidationSource,
        ExtendedSingleValueValidatedValueObjectFake
    >
{
    public static readonly string LessThanZeroValueErrorMessage =
        "Value must be greater than zero.";
    public static readonly string GreaterThanTenValueErrorMessage = "Value must less than 10.";
    public static readonly string MinusFiveValueErrorMessage = "Forbidden value occured.";
    public static readonly string LessThanZeroNextValueErrorMessage =
        "Value must be greater than zero.";

    protected override SingleValueValueObjectValidationSource ValidationSource { get; }

    public ExtendedSingleValueValueObjectValidatorFake()
        : base()
    {
        this.ValidationSource = new SingleValueValueObjectValidationSource();

        _ = this
            .Configuration.WithValidation(
                nameof(SingleValueValueObjectValidationSource.Value),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.Value < 0
                                ? new ValidationError(
                                    nameof(SingleValueValueObjectValidationSource.Value),
                                    LessThanZeroValueErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.Value > 10
                                ? new ValidationError(
                                    nameof(SingleValueValueObjectValidationSource.Value),
                                    GreaterThanTenValueErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.Value == -5
                                ? new ValidationError(
                                    nameof(SingleValueValueObjectValidationSource.Value),
                                    MinusFiveValueErrorMessage
                                )
                                : null
                        )
            )
            .WithValidation(
                nameof(SingleValueValueObjectValidationSource.NextValue),
                source =>
                    source.NextValue < 0
                        ? new ValidationError(
                            nameof(SingleValueValueObjectValidationSource.NextValue),
                            LessThanZeroNextValueErrorMessage
                        )
                        : null
            );
    }

    protected override void UpdateSource(
        ExtendedSingleValueValidatedValueObjectFake validationTarget
    ) => this.ValidationSource.Value = validationTarget.Value;
}
