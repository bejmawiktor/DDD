﻿using DDD.Domain.Validation;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.TestDoubles;

internal class AggregateRootValidatorFake
    : DomainObjectValidator<AggregateRootValidatorFake, ValidatedAggregateRootFake>
{
    public static readonly string EmptyTextFieldErrorMessage =
        "Empty TextField given. This field is required.";
    public static readonly string LessThanZeroIntFieldErrorMessage =
        "IntField must be greater than zero.";
    public static readonly string GreaterThanTenIntFieldErrorMessage =
        "IntField must less than 10.";

    public static readonly string MinusFiveIntFieldErrorMessage = "Forbidden value occured.";

    public string? TextField { get; set; }
    public int? IntField { get; set; }

    protected override AggregateRootValidatorFake ValidationSource => this;

    public AggregateRootValidatorFake(string textField, int intField)
        : this()
    {
        this.TextField = textField;
        this.IntField = intField;
    }

    public AggregateRootValidatorFake()
        : base()
    {
        _ = this
            .Configuration.WithValidation(
                nameof(this.TextField),
                value =>
                    value.TextField == string.Empty
                        ? new ValidationError(nameof(this.TextField), EmptyTextFieldErrorMessage)
                        : null
            )
            .WithValidation(
                nameof(this.IntField),
                validator =>
                    validator
                        .WithValidationStep(value =>
                            value.IntField < 0
                                ? new ValidationError(
                                    nameof(this.IntField),
                                    LessThanZeroIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField > 10
                                ? new ValidationError(
                                    nameof(this.IntField),
                                    GreaterThanTenIntFieldErrorMessage
                                )
                                : null
                        )
                        .WithValidationStep(value =>
                            value.IntField == -5
                                ? new ValidationError(
                                    nameof(this.IntField),
                                    MinusFiveIntFieldErrorMessage
                                )
                                : null
                        )
            );
    }

    protected override void UpdateSource(ValidatedAggregateRootFake validationTarget)
    {
        this.TextField = validationTarget.TextField;
        this.IntField = validationTarget.IntField;
    }
}
