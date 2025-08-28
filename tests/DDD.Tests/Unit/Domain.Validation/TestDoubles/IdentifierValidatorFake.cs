using DDD.Domain.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class IdentifierValidatorFake
    : DomainObjectValidator<IdentifierValidatorFake, ValidatedIdentifierFake>
{
    public static readonly string EmptyValueErrorMessage =
        "Empty Value given. Identifier can't be empty.";

    public static readonly string ValueCantBeABErrorMessage = "Identifier can't be ab.";

    public static readonly string ValueLengthGreaterThanOndeErrorMessage =
        "Identifier can't be longer than 1 character.";

    public string? Value { get; set; }

    protected override IdentifierValidatorFake ValidationSource => this;

    public IdentifierValidatorFake()
        : base()
    {
        _ = this.Configuration.WithValidation(
            nameof(this.Value),
            validator =>
                validator
                    .WithValidationStep(value =>
                        value.Value == string.Empty
                            ? new ValidationError(nameof(this.Value), EmptyValueErrorMessage)
                            : null
                    )
                    .WithValidationStep(value =>
                        value.Value == "AB"
                            ? new ValidationError(nameof(this.Value), ValueCantBeABErrorMessage)
                            : null
                    )
                    .WithValidationStep(value =>
                        value.Value?.Length > 1
                            ? new ValidationError(
                                nameof(this.Value),
                                ValueLengthGreaterThanOndeErrorMessage
                            )
                            : null
                    )
        );
    }

    protected override void UpdateSource(ValidatedIdentifierFake validationTarget) =>
        this.Value = validationTarget.Value;
}
