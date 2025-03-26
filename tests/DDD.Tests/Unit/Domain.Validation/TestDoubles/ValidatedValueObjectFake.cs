using System.Collections.Generic;
using DDD.Domain.Validation;
using Utils.Functional;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

public class ValidatedValueObjectFake
    : ValueObject<ValidatedValueObjectFake, ValueObjectValidatorFake, ValueObjectValidatorFake>
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

    public ValidatedValueObjectFake(string textField, int intField)
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

    protected override IEnumerable<object?> GetEqualityMembers()
    {
        yield return this.TextField;
        yield return this.IntField;
    }
}
