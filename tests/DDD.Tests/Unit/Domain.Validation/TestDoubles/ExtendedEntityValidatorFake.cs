using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class EntityValidationSource
{
    public string? TextField { get; set; }
    public int? IntField { get; set; }
}

internal class ExtendedEntityValidatorFake
    : DomainObjectValidator<EntityValidationSource, ExtendedValidatedEntityFake>
{
    public static readonly string EmptyTextFieldErrorMessage =
        "Empty TextField given. This field is required.";
    public static readonly string LessThanZeroIntFieldErrorMessage =
        "IntField must be greater than zero.";
    public static readonly string GreaterThanTenIntFieldErrorMessage =
        "IntField must less than 10.";

    public static readonly string MinusFiveIntFieldErrorMessage = "Forbidden value occured.";

    protected override EntityValidationSource ValidationSource { get; }

    public ExtendedEntityValidatorFake()
        : base()
    {
        this.ValidationSource = new EntityValidationSource();

        _ = this
            .Configuration.WithValidation(
                nameof(EntityValidationSource.TextField),
                value =>
                    value.TextField == string.Empty
                        ? new ValidationError(
                            nameof(EntityValidationSource.TextField),
                            EmptyTextFieldErrorMessage
                        )
                        : null
            )
            .WithValidation(
                nameof(EntityValidationSource.IntField),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.IntField < 0
                                ? new ValidationError(
                                    nameof(EntityValidationSource.IntField),
                                    LessThanZeroIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField > 10
                                ? new ValidationError(
                                    nameof(EntityValidationSource.IntField),
                                    GreaterThanTenIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField == -5
                                ? new ValidationError(
                                    nameof(EntityValidationSource.IntField),
                                    MinusFiveIntFieldErrorMessage
                                )
                                : null
                        )
            );
    }

    protected override void UpdateSource(ExtendedValidatedEntityFake validationTarget)
    {
        this.ValidationSource.TextField = validationTarget.TextField;
        this.ValidationSource.IntField = validationTarget.IntField;
    }
}
