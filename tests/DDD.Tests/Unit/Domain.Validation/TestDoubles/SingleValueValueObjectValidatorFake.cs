using DDD.Domain.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

public class SingleValueValueObjectValidatorFake
    : DomainObjectValidator<
        SingleValueValueObjectValidatorFake,
        SingleValueValidatedValueObjectFake
    >
{
    public static readonly string LessThanZeroValueErrorMessage =
        "Value must be greater than zero.";
    public static readonly string GreaterThanTenValueErrorMessage = "Value must less than 10.";
    public static readonly string MinusFiveValueErrorMessage = "Forbidden value occured.";
    public static readonly string LessThanZeroNextValueErrorMessage =
        "Value must be greater than zero.";

    public int? Value { get; set; }
    public int? NextValue { get; set; }

    protected override SingleValueValueObjectValidatorFake ValidationSource => this;

    public SingleValueValueObjectValidatorFake(int value)
        : this()
    {
        this.Value = value;
    }

    public SingleValueValueObjectValidatorFake()
        : base()
    {
        _ = this
            .Configuration.WithValidation(
                nameof(this.Value),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.Value < 0
                                ? new ValidationError(
                                    nameof(this.Value),
                                    LessThanZeroValueErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.Value > 10
                                ? new ValidationError(
                                    nameof(this.Value),
                                    GreaterThanTenValueErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.Value == -5
                                ? new ValidationError(
                                    nameof(this.Value),
                                    MinusFiveValueErrorMessage
                                )
                                : null
                        )
            )
            .WithValidation(
                nameof(this.NextValue),
                source =>
                    source.NextValue < 0
                        ? new ValidationError(
                            nameof(this.NextValue),
                            LessThanZeroNextValueErrorMessage
                        )
                        : null
            );
    }

    protected override void UpdateSource(SingleValueValidatedValueObjectFake validationTarget) =>
        this.Value = validationTarget.Value;
}
