using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Tests.Unit.Domain.Validation.TestDoubles;
using NUnit.Framework;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
internal class EntityTest
{
    public static IEnumerable<TestCaseData> CreateIncorrectDataTestData(string testName)
    {
        yield return new TestCaseData(
            "",
            5,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.TextField),
                    EntityValidatorFake.EmptyTextFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            -1,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
            "example text",
            -5,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.MinusFiveIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            11,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
        yield return new TestCaseData(
            "",
            11,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.TextField),
                    EntityValidatorFake.EmptyTextFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField and Greater Than Ten IntField)");
        yield return new TestCaseData(
            "",
            -11,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.TextField),
                    EntityValidatorFake.EmptyTextFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField and Less Than Zero IntField)");
        yield return new TestCaseData(
            "",
            -5,
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.TextField),
                    EntityValidatorFake.EmptyTextFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.MinusFiveIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField and Less Than Zero Forbidden Value IntField)");
    }

    public static IEnumerable<TestCaseData> CreateIncorrectDataForOneOfValidatorsTestData(
        string testName
    )
    {
        yield return new TestCaseData(
            "my example text",
            5,
            (ValidatedEntityFake entity) =>
            {
                entity.TextField = "";
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.TextField),
                    EntityValidatorFake.EmptyTextFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedEntityFake entity) =>
            {
                entity.IntField = -1;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedEntityFake entity) =>
            {
                entity.IntField = -5;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.LessThanZeroIntFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.MinusFiveIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ValidatedEntityFake entity) =>
            {
                entity.IntField = 11;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidatorFake.IntField),
                    EntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
    }

    public static IEnumerable<TestCaseData> CreateIncorrectDataForOneOfExtendedValidatorsTestData(
        string testName
    )
    {
        yield return new TestCaseData(
            "my example text",
            5,
            (ExtendedValidatedEntityFake entity) =>
            {
                entity.TextField = "";
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidationSource.TextField),
                    ExtendedEntityValidatorFake.EmptyTextFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Empty TextField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ExtendedValidatedEntityFake entity) =>
            {
                entity.IntField = -1;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidationSource.IntField),
                    ExtendedEntityValidatorFake.LessThanZeroIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ExtendedValidatedEntityFake entity) =>
            {
                entity.IntField = -5;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidationSource.IntField),
                    ExtendedEntityValidatorFake.LessThanZeroIntFieldErrorMessage
                ),
                new ValidationException(
                    nameof(EntityValidationSource.IntField),
                    ExtendedEntityValidatorFake.MinusFiveIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Less Than Zero And Forbbiden Value IntField)");
        yield return new TestCaseData(
            "example text",
            5,
            (ExtendedValidatedEntityFake entity) =>
            {
                entity.IntField = 11;
            },
            new AggregateException(
                new ValidationException(
                    nameof(EntityValidationSource.IntField),
                    ExtendedEntityValidatorFake.GreaterThanTenIntFieldErrorMessage
                )
            )
        ).SetName($"{testName}(Greater Than Ten IntField)");
    }

    [Test]
    public void TestConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedEntityFake entity = new(10, "example text", 5);

        Assert.Multiple(() =>
        {
            Assert.That(entity.TextField, Is.EqualTo("example text"));
            Assert.That(entity.IntField, Is.EqualTo(5));
        });
    }

    [Test]
    public void TestExtendedConstructing_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedEntityFake entity = new(10, "example text", 5);

        Assert.Multiple(() =>
        {
            Assert.That(entity.TextField, Is.EqualTo("example text"));
            Assert.That(entity.IntField, Is.EqualTo(5));
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataTestData),
        new object[] { nameof(TestValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown) }
    )]
    public void TestValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ValidatedEntityFake(10, textField, intField)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataTestData),
        new object[]
        {
            nameof(TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown),
        }
    )]
    public void TestExtendedValidate_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        AggregateException aggregateException
    )
    {
        AggregateException? exception = Assert.Throws<AggregateException>(
            () => new ExtendedValidatedEntityFake(10, textField, intField)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }

    [Test]
    public void TestValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ValidatedEntityFake entity =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        Assert.Multiple(() =>
        {
            Assert.That(entity.TextField, Is.EqualTo("second text"));
            Assert.That(entity.IntField, Is.EqualTo(5));
        });
    }

    [Test]
    public void TestExtendedValidationWithOneOfValidators_WhenCorrectDataGiven_ThenNoExceptionIsThrown()
    {
        ExtendedValidatedEntityFake entity =
            new(10, "example text", 5) { TextField = "second text", IntField = 5 };

        Assert.Multiple(() =>
        {
            Assert.That(entity.TextField, Is.EqualTo("second text"));
            Assert.That(entity.IntField, Is.EqualTo(5));
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataForOneOfValidatorsTestData),
        new object[]
        {
            nameof(
                TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ValidatedEntityFake> updateAction,
        AggregateException aggregateException
    )
    {
        ValidatedEntityFake entity = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(entity)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }

    [TestCaseSource(
        nameof(CreateIncorrectDataForOneOfExtendedValidatorsTestData),
        new object[]
        {
            nameof(
                TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown
            ),
        }
    )]
    public void TestExtendedValidateWithOneOfValidators_WhenIncorrectDataGiven_ThenAggregateExceptionIsThrown(
        string textField,
        int intField,
        Action<ExtendedValidatedEntityFake> updateAction,
        AggregateException aggregateException
    )
    {
        ExtendedValidatedEntityFake entity = new(10, textField, intField);

        AggregateException? exception = Assert.Throws<AggregateException>(
            () => updateAction(entity)
        );

        Assert.Multiple(() =>
        {
            Assert.That(exception, Is.Not.Null);
            Assert.That(
                exception?.Flatten().InnerExceptions.Select(exception => exception.ToString()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.ToString())
                )
            );
            Assert.That(
                exception?.InnerExceptions.Select(exception => exception.GetType()),
                Is.EquivalentTo(
                    aggregateException
                        .Flatten()
                        .InnerExceptions.Select(exception => exception.GetType())
                )
            );
        });
    }
}
