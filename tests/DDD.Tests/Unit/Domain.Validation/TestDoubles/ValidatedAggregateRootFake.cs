using DDD.Domain.Validation;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class ValidatedAggregateRootFake
    : AggregateRoot<
        ValidatedAggregateRootFake,
        int,
        AggregateRootValidatorFake,
        AggregateRootValidatorFake
    >
{
    private string textField;
    private int intField;

    public string TextField
    {
        get => this.textField;
        set
        {
            this.ValidateTextField(value);

            this.textField = value;
        }
    }

    private void ValidateTextField(string value) =>
        this.Validate(nameof(this.TextField), source => source.TextField = value).ThrowIfFailed();

    public int IntField
    {
        get => this.intField;
        set
        {
            this.ValidateIntField(value);

            this.intField = value;
        }
    }

    private void ValidateIntField(int value) =>
        this.Validate(nameof(this.IntField), source => source.IntField = value).ThrowIfFailed();

    public ValidatedAggregateRootFake(int id, string textField, int intField)
        : base(id)
    {
        this.ValidateMembers(textField, intField);

        this.textField = textField;
        this.intField = intField;
    }

    private void ValidateMembers(string textField, int intField)
    {
        this.Validate(source =>
            {
                source.TextField = textField;
                source.IntField = intField;
            })
            .ThrowIfFailed();
    }
}
