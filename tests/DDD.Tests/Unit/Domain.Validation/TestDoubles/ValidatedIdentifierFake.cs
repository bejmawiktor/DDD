using DDD.Domain.Model;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class ValidatedIdentifierFake
    : Identifier<ValidatedIdentifierFake, string, IdentifierValidatorFake>
{
    public ValidatedIdentifierFake(string value)
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
