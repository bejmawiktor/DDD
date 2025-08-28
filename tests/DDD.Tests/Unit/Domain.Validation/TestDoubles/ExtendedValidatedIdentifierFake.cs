using DDD.Domain.Model.Extended;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class ExtendedValidatedIdentifierFake
    : Identifier<
        ExtendedValidatedIdentifierFake,
        string,
        ExtendedIdentifierValidatorFake,
        IdentifierValidationSource
    >
{
    public ExtendedValidatedIdentifierFake(string value)
        : base(value) { }

    protected override void ValidateValue(string value)
    {
        this.Validator.Validate(source =>
            {
                source.Value = value;
            })
            .ThrowIfFailed();
    }
}
