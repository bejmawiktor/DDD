using DDD.Domain.Model.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class IdentifierValidationSource
{
    public string? Value { get; set; }
}

internal class ExtendedIdentifierValidatorFake
    : DomainObjectValidator<IdentifierValidationSource, ExtendedValidatedIdentifierFake>
{
    public static readonly string EmptyValueErrorMessage =
        "Empty Value given. Identifier can't be empty.";

    public static readonly string ValueCantBeABErrorMessage = "Identifier can't be ab.";

    public static readonly string ValueLengthGreaterThanOndeErrorMessage =
        "Identifier can't be longer than 1 character.";

    protected override IdentifierValidationSource ValidationSource { get; }

    public ExtendedIdentifierValidatorFake()
        : base()
    {
        this.ValidationSource = new IdentifierValidationSource();

        _ = this.Configuration.WithValidation(
            nameof(IdentifierValidationSource.Value),
            validator =>
                validator
                    .WithValidationStep(value =>
                        value.Value == string.Empty
                            ? new ValidationError(
                                nameof(IdentifierValidationSource.Value),
                                EmptyValueErrorMessage
                            )
                            : null
                    )
                    .WithValidationStep(value =>
                        value.Value == "AB"
                            ? new ValidationError(
                                nameof(IdentifierValidationSource.Value),
                                ValueCantBeABErrorMessage
                            )
                            : null
                    )
                    .WithValidationStep(value =>
                        value.Value?.Length > 1
                            ? new ValidationError(
                                nameof(IdentifierValidationSource.Value),
                                ValueLengthGreaterThanOndeErrorMessage
                            )
                            : null
                    )
        );
    }

    protected override void UpdateSource(ExtendedValidatedIdentifierFake validationTarget) =>
        this.ValidationSource.Value = validationTarget.Value;
}
