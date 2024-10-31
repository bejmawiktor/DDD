using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.Domain.Validation;
using DDD.Tests.Unit.Domain.TestDoubles;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validator;

using Validator = DDD.Domain.Validation.Validator;

[TestFixture]
public class ValidatorTest
{
    public static IEnumerable<TestCaseData> MultipleExceptionsTestCase
    {
        get
        {
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new("exception"), new ArgumentNullException("argument") },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(1)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentNullException("argument"),
                    },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(2)"
            );
            yield return new TestCaseData(
                new object[]
                {
                    new Exception[]
                    {
                        new InvalidOperationException("invalid operation"),
                        new ArgumentException("argument"),
                        new("exception"),
                    },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(3)"
            );

            yield return new TestCaseData(
                new object[]
                {
                    new Exception[] { new InvalidOperationException("invalid operation") },
                }
            ).SetName(
                $"{nameof(TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull)}(4)"
            );
        }
    }

    public static IEnumerable<object> SuccessResultsTestCase => ["result", 1, new IntIdFake(12)];

    [Test]
    public void TestThrow_WhenUsingWithoutScope_ThenThrowException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<Exception>().And.Message.EqualTo("exception"),
            () => Validator.Throw(new Exception("exception"))
        );
        _ = Assert.Throws(
            Is.InstanceOf<InvalidOperationException>().And.Message.EqualTo("Invalid operation"),
            () => Validator.Throw(new InvalidOperationException("Invalid operation"))
        );
    }

    [Test]
    public void TestThrow_WhenUsingWithinValidateScope_ThenDontThrowException()
    {
        Assert.DoesNotThrow(
            () =>
                Validator.ValidateMany<object>(() =>
                {
                    Validator.Throw(new InvalidOperationException("exception"));
                    return new object();
                })
        );
    }

    [Test]
    public void TestThrow_WhenGivingNullInsteadOfException_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exception"),
            () => Validator.Throw<Exception>(null!)
        );
    }

    [Test]
    public void TestThrow_WhenGivingNullInsteadOfExceptionWithinValidationScope_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("exception"),
            () =>
                Validator.ValidateMany(() =>
                {
                    Validator.Throw<Exception>(null!);

                    return new object();
                })
        );
    }

    [Test]
    public void TestValidateMany_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationAction"),
            () => Validator.ValidateMany((null as Action)!)
        );
    }

    [Test]
    public void TestValidateManyWithResult_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.Throws(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            () => Validator.ValidateMany<object>((null as Func<object>)!)
        );
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public void TestValidateMany_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult result = Validator.ValidateMany(() =>
        {
            exceptions.ToList().ForEach(exception => Validator.Throw(exception));
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(result.Exceptions!));
            Assert.That(result, Is.EqualTo(ValidationResult.Failure));
        });
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public void TestValidateManyWithResult_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<object> result = Validator.ValidateMany(() =>
        {
            exceptions.ToList().ForEach(exception => Validator.Throw(exception));

            return new object();
        });

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(result.Exceptions!));
            Assert.That(result, Is.EqualTo(ValidationResult.Failure));
            Assert.That(result.Result, Is.EqualTo(null));
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public void TestValidateMany_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSuccessfull(
        object value
    )
    {
        ValidationResult result = Validator.ValidateMany(() => { });

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public void TestValidateManyWithResult_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSuccessfull(
        object value
    )
    {
        ValidationResult<object> result = Validator.ValidateMany(() => value);

        Assert.Multiple(() =>
        {
            Assert.That(value, Is.EqualTo(result.Result));
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [Test]
    public void TestValidateManyAsync_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            async () => await Validator.ValidateManyAsync((null as Func<Task>)!)
        );
    }

    [Test]
    public void TestValidateManyAsyncWithResult_WhenGivingNullInsteadOfValidationFunc_ThenThrowArgumentNullException()
    {
        _ = Assert.ThrowsAsync(
            Is.InstanceOf<ArgumentNullException>()
                .And.Property(nameof(ArgumentNullException.ParamName))
                .EqualTo("validationFunc"),
            async () => await Validator.ValidateManyAsync<object>((null as Func<Task<object>>)!)
        );
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public async Task TestValidateManyAsync_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult result = await Validator.ValidateManyAsync(
            () =>
                Task.Run(() =>
                {
                    exceptions.ToList().ForEach(exception => Validator.Throw(exception));
                })
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(result.Exceptions!));
            Assert.That(result, Is.EqualTo(ValidationResult.Failure));
        });
    }

    [TestCaseSource(nameof(MultipleExceptionsTestCase))]
    public async Task TestValidateManyAsyncWithResult_WhenGivingValidationFuncWithMultipleThrow_ThenReturnAllExceptionsAndValidationResultIsntSuccessFull(
        IEnumerable<Exception> exceptions
    )
    {
        ValidationResult<object> result = await Validator.ValidateManyAsync(
            () =>
                Task.Run(() =>
                {
                    exceptions.ToList().ForEach(exception => Validator.Throw(exception));

                    return new object();
                })
        );

        Assert.Multiple(() =>
        {
            Assert.That(result.Exceptions, Is.EquivalentTo(result.Exceptions!));
            Assert.That(result, Is.EqualTo(ValidationResult.Failure));
            Assert.That(result.Result, Is.EqualTo(null));
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public async Task TestValidateManyAsync_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSucessfull(
        object value
    )
    {
        ValidationResult result = await Validator.ValidateManyAsync(() => Task.CompletedTask);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }

    [TestCaseSource(nameof(SuccessResultsTestCase))]
    public async Task TestValidateManyAsyncWithResult_WhenGivingValidationFuncWithoutThrow_ThenReturnValueAndValidationResultIsSucessfull(
        object value
    )
    {
        ValidationResult<object> result = await Validator.ValidateManyAsync(
            () => Task.FromResult(value)
        );

        Assert.Multiple(() =>
        {
            Assert.That(value, Is.EqualTo(result.Result));
            Assert.That(result, Is.EqualTo(ValidationResult.Success));
            Assert.That(result.Exceptions, Is.Null);
        });
    }
}
