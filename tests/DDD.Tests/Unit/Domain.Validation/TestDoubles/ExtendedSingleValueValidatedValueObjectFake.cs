using DDD.Domain.Model.Extended;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

public class ExtendedSingleValueValidatedValueObjectFake
    : ValueObject<
        ExtendedSingleValueValidatedValueObjectFake,
        int,
        ExtendedSingleValueValueObjectValidatorFake,
        SingleValueValueObjectValidationSource
    >
{
    private int nextValue;
    public new int Value => base.Value;
    public int NextValue
    {
        get => this.nextValue;
        set
        {
            this.ValidateNextValue(value);
            this.nextValue = value;
        }
    }

    public ExtendedSingleValueValidatedValueObjectFake(int value, int nextValue)
        : base(value)
    {
        this.ValidateNextValue(nextValue);

        this.nextValue = nextValue;
    }

    private void ValidateNextValue(int nextValue) =>
        this
            .Validator.Validate(nameof(this.NextValue), source => source.NextValue = nextValue)
            .ThrowIfFailed();

    protected override void ValidateValue(int value)
    {
        this.Validator.Validate(source =>
            {
                source.Value = value;
            })
            .ThrowIfFailed();
    }
}
