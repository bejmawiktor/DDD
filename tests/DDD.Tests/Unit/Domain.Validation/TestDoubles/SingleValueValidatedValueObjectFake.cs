using DDD.Domain.Validation;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

public class SingleValueValidatedValueObjectFake
    : ValueObject<
        SingleValueValidatedValueObjectFake,
        int,
        SingleValueValueObjectValidatorFake,
        SingleValueValueObjectValidatorFake
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

    public SingleValueValidatedValueObjectFake(int value, int nextValue)
        : base(value)
    {
        this.ValidateNextValue(nextValue);

        this.nextValue = nextValue;
    }

    private void ValidateNextValue(int nextValue) =>
        this.Validate(source => source.NextValue = nextValue).ThrowIfFailed();

    protected override void ValidateValue(int value)
    {
        this.Validate(source =>
            {
                source.Value = value;
            })
            .ThrowIfFailed();
    }
}
