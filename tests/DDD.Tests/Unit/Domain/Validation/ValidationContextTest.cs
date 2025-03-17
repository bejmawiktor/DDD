using System;
using System.Collections.Generic;
using System.Linq;
using DDD.Domain.Validation;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation;

[TestFixture]
public class ValidationContextTest
{
    public static IEnumerable<TestCaseData> ThrowExceptionTestData
    {
        get
        {
            ValidationException convertedValidationException = new("NewFieldName", "NewMessage");
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    convertedValidationException,
                }
            ).SetName(
                $"{nameof(TestThrowingValidationException_WhenValidationContextIsUsed_ThenConvertedValidationExceptionIsThrown)}(Single)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => new ValidationException(
                                $"${validationException.FieldName}_2",
                                $"{validationException.Message}_2"
                            ));

                        validationContext.Dispose();

                        using ValidationContext<ValidationException> validationContext2 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    convertedValidationException,
                }
            ).SetName(
                $"{nameof(TestThrowingValidationException_WhenValidationContextIsUsed_ThenConvertedValidationExceptionIsThrown)}(Multiple)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => new ValidationException(
                                $"${validationException.FieldName}_2",
                                $"{validationException.Message}_2"
                            ));
                        using ValidationContext<ValidationException> validationContext2 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException(
                        $"${convertedValidationException.FieldName}_2",
                        $"{convertedValidationException.Message}_2"
                    ),
                }
            ).SetName(
                $"{nameof(TestThrowingValidationException_WhenValidationContextIsUsed_ThenConvertedValidationExceptionIsThrown)}(Nested)"
            );
        }
    }

    public static IEnumerable<TestCaseData> ValidateManyTestData
    {
        get
        {
            ValidationException convertedValidationException = new("NewFieldName", "NewMessage");

            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException[] { convertedValidationException },
                }
            ).SetName(
                $"{nameof(TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned)}(Single)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => new ValidationException(
                                $"${validationException.FieldName}_2",
                                $"{validationException.Message}_2"
                            ));

                        validationContext.Dispose();

                        using ValidationContext<ValidationException> validationContext2 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException[] { convertedValidationException },
                }
            ).SetName(
                $"{nameof(TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned)}(Multiple With First Empty)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => new ValidationException(
                                $"{validationException.FieldName}_2",
                                $"{validationException.Message}_2"
                            ));

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );

                        validationContext.Dispose();

                        using ValidationContext<ValidationException> validationContext2 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException[]
                    {
                        new("defaultFieldName_2", "Example exception_2"),
                        convertedValidationException,
                    },
                }
            ).SetName(
                $"{nameof(TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned)}(Multiple With Two)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using ValidationContext<ValidationException> validationContext =
                            new(validationException => new ValidationException(
                                $"{validationException.FieldName}_2",
                                $"{validationException.Message}_2"
                            ));
                        using ValidationContext<ValidationException> validationContext2 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException[]
                    {
                        new(
                            $"{convertedValidationException.FieldName}_2",
                            $"{convertedValidationException.Message}_2"
                        ),
                    },
                }
            ).SetName(
                $"{nameof(TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned)}(Nested)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    () =>
                    {
                        using (
                            ValidationContext<ValidationException> validationContext =
                                new(validationException => new ValidationException(
                                    $"{validationException.FieldName}_Parent",
                                    $"{validationException.Message}_Parent"
                                ))
                        )
                        {
                            using (
                                ValidationContext<ValidationException> validationContext1 =
                                    new(validationException => new ValidationException(
                                        $"{validationException.FieldName}_Child1",
                                        $"{validationException.Message}_Child1"
                                    ))
                            )
                            {
                                Validator<ValidationException>.Throw(
                                    new ValidationException("defaultFieldName", "Example exception")
                                );
                            }

                            using ValidationContext<ValidationException> validationContext2 =
                                new(validationException => convertedValidationException);
                            Validator<ValidationException>.Throw(
                                new ValidationException("defaultFieldName", "Example exception")
                            );
                        }

                        using ValidationContext<ValidationException> validationContext3 =
                            new(validationException => convertedValidationException);

                        Validator<ValidationException>.Throw(
                            new ValidationException("defaultFieldName", "Example exception")
                        );
                    },
                    new ValidationException[]
                    {
                        new("defaultFieldName_Child1_Parent", "Example exception_Child1_Parent"),
                        new(
                            $"{convertedValidationException.FieldName}_Parent",
                            $"{convertedValidationException.Message}_Parent"
                        ),
                        convertedValidationException,
                    },
                }
            ).SetName(
                $"{nameof(TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned)}(Nested Multiple)"
            );
        }
    }

    [Test]
    public void TestConstructing_WhenNullValidationExceptionConverterGiven_ThenArgumentNullExceptionIsThrown()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exceptionConverter"),
            () => new ValidationContext<ValidationException>(null!)
        );
    }

    [Test]
    public void TestConstructing_WhenValidationExceptionConverterGiven_ThenValidationExceptionConverterIsSet()
    {
        ValidationException convertedValidationException = new("NewFieldName", "NewMessage");
        ValidationContext<ValidationException> validationContext =
            new(validationException => convertedValidationException);

        ValidationException convertedValidationExceptionResult =
            validationContext.ExceptionConverter(new ValidationException("Default Message"));

        Assert.That(
            convertedValidationExceptionResult.ToString(),
            Is.EqualTo(convertedValidationException.ToString())
        );
    }

    [TestCaseSource(nameof(ThrowExceptionTestData))]
    public void TestThrowingValidationException_WhenValidationContextIsUsed_ThenConvertedValidationExceptionIsThrown(
        Action exceptionThrowAction,
        ValidationException convertedValidationException
    )
    {
        _ = Assert.Throws(
            Is.InstanceOf<ValidationException>()
                .And.Property(nameof(ValidationException.FieldName))
                .EqualTo(convertedValidationException.FieldName)
                .And.Property(nameof(Exception.Message))
                .EqualTo(convertedValidationException.Message),
            () => exceptionThrowAction()
        );
    }

    [TestCaseSource(nameof(ValidateManyTestData))]
    public void TestValidatingMany_WhenValidationContextIsUsed_ThenConvertedValidationErrorIsReturned(
        Action exceptionThrowAction,
        ValidationException[] validationExceptions
    )
    {
        ValidationResult<ValidationException> result = Validator<ValidationException>.ValidateMany(
            exceptionThrowAction
        );

        Assert.That(
            result.Exceptions?.Select(exception => exception.ToString()),
            Is.EquivalentTo(validationExceptions.Select(exception => exception.ToString()))
        );
    }

    [Test]
    public void TestThrowingValidationException_WhenValidationContextIsNotUsed_ThenValidationExceptionIsThrown()
    {
        ValidationException validationException = new("Field", "Example exception");

        _ = Assert.Throws(
            Is.EqualTo(validationException),
            () =>
            {
                Validator<Exception>.Throw(validationException);
            }
        );
    }

    [Test]
    public void TestValidateMany_WhenValidationContextIsNotUsed_ThenValidationErrorIsReturned()
    {
        ValidationException validationException = new("Field", "Example exception");
        ValidationResult<ValidationException> result = Validator<ValidationException>.ValidateMany(
            () => Validator<ValidationException>.Throw(validationException)
        );

        Assert.That(
            result.Exceptions?.Select(exception => exception.ToString()),
            Is.EquivalentTo(new string[] { validationException.ToString() })
        );
    }
}
