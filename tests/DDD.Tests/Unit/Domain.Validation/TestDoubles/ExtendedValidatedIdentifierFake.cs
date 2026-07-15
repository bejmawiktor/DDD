using DDD.Domain.Model.Extended;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class ExtendedValidatedIdentifierFake(string value)
    : Identifier<
        string,
        ExtendedValidatedIdentifierFake,
        ExtendedIdentifierValidatorFake,
        IdentifierValidationSource
    >(value)
{
    protected override void ValidateValue(string value)
    {
        this.Validator.Validate(source =>
            {
                source.Value = value;
            })
            .ThrowIfFailed();
    }
}
