using System.Diagnostics;
using System.Net;
using DDD.Domain.Validation.AspNetCore;
using DDD.Tests.Unit.Domain.Validation.AspNetCore.TestDoubles;
using DDD.Tests.Unit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utils.Functional;
using Utils.Validation;
using ErrorCase = (
    string? Path,
    string Error,
    Microsoft.AspNetCore.Mvc.ProblemDetails ExpectedProblemDetails
);
using ErrorWithMetadataCase = (
    string? Path,
    string Message,
    System.Collections.Generic.IReadOnlyDictionary<string, object?> Metadata,
    Microsoft.AspNetCore.Mvc.ProblemDetails ExpectedProblemDetails
);
using ErrorWithReasonsCase = (
    string? Path,
    object Error,
    Microsoft.AspNetCore.Mvc.ProblemDetails ExpectedProblemDetails
);

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

public class ErrorExtensionsTest
{
    public static IEnumerable<
        Func<TestDataRow<ErrorWithReasonsCase>>
    > CreateErrorWithReasonsTestData()
    {
        ValidationError messageOnlyValidationError = new("my error");
        ValidationError withFieldValidationError = new("fieldName", "my validation error");
        ValidationError withField2ValidationError = new("fieldName", "my validation error 2");
        ValidationError secondFieldValidationError = new("fieldName2", "my validation error 2");
        ValidationError secondField2ValidationError = new("fieldName2", "my validation error 3");
        Error argumentError = new("my argument error");
        Error simpleError = new("simple error");
        NotFoundError notFoundError = new("not found error");
        NotFoundError secondNotFoundError = new("not found exception 2");
        ValidationError nestedValidationError = new(
            "nestedFieldName",
            "my nested validation error"
        );
        ValidationError deeplyNestedValidationError = new(
            "fieldName",
            "my deeply nested validation error"
        );
        ValidationError fieldWithMetadataValidationError = new(
            "fieldName",
            "my validation error with metadata",
            new Dictionary<string, object?>() { { "errorCode", "E-100" } }
        );
        Error nestedSimpleError = new("nested simple error");
        Error attemptsMetadataError = new(
            "my metadata error",
            new Dictionary<string, object?>() { { "attempts", 3 } }
        );
        Error codeAndRetryableMetadataError = new(
            "my metadata error 2",
            new Dictionary<string, object?>() { { "errorCode", "E-001" }, { "retryable", true } }
        );
        Error firstCodeMetadataError = new(
            "my metadata error 3",
            new Dictionary<string, object?>() { { "errorCode", "E-001" } }
        );
        Error secondCodeMetadataError = new(
            "my metadata error 4",
            new Dictionary<string, object?>() { { "errorCode", "E-002" }, { "attempts", 3 } }
        );
        Error thirdCodeMetadataError = new(
            "my metadata error 5",
            new Dictionary<string, object?>() { { "errorCode", "E-003" } }
        );
        Error traceIdMetadataError = new(
            "my metadata error 6",
            new Dictionary<string, object?>() { { "traceId", "spoofed" } }
        );
        Error nestedCodeMetadataError = new(
            "my nested metadata error",
            new Dictionary<string, object?>() { { "errorCode", "E-002" } }
        );

        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(messageOnlyValidationError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [messageOnlyValidationError.Message] },
                    }
                )
                {
                    Detail = messageOnlyValidationError.Message,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Single validation error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(withFieldValidationError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { withFieldValidationError.FieldName!, [withFieldValidationError.Message] },
                    }
                )
                {
                    Detail = withFieldValidationError.Message,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Field name validation error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                null,
                new AggregateError<IError>(simpleError, argumentError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = null,
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Two errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(
                    withFieldValidationError,
                    simpleError,
                    argumentError,
                    messageOnlyValidationError,
                    notFoundError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",
                            [
                                simpleError.Message,
                                argumentError.Message,
                                messageOnlyValidationError.Message,
                                notFoundError.Message,
                            ]
                        },
                        { withFieldValidationError.FieldName, [withFieldValidationError.Message] },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                      - {messageOnlyValidationError.Message}
                      - {notFoundError.Message}
                    """,
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple errors with field name"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    withField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Same field name errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    secondFieldValidationError,
                    withField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                        {
                            secondFieldValidationError.FieldName,
                            [secondFieldValidationError.Message]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {secondFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldValidationError,
                    secondFieldValidationError,
                    withField2ValidationError,
                    secondField2ValidationError,
                    simpleError,
                    argumentError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [simpleError.Message, argumentError.Message] },
                        {
                            withFieldValidationError.FieldName,
                            [withFieldValidationError.Message, withField2ValidationError.Message]
                        },
                        {
                            secondFieldValidationError.FieldName,
                            [
                                secondFieldValidationError.Message,
                                secondField2ValidationError.Message,
                            ]
                        },
                    }
                )
                {
                    Detail = $"""
                    Multiple errors found:
                      - {withFieldValidationError.Message}
                      - {secondFieldValidationError.Message}
                      - {withField2ValidationError.Message}
                      - {secondField2ValidationError.Message}
                      - {simpleError.Message}
                      - {argumentError.Message}
                    """,
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors with multiple messages"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError),
                new ProblemDetails()
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.NotFound,
                }
            ),
            "Single not found error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError, secondNotFoundError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [notFoundError.Message, secondNotFoundError.Message] },
                    }
                )
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple not found errors"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    attemptsMetadataError,
                    simpleError,
                    codeAndRetryableMetadataError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",
                            [
                                attemptsMetadataError.Message,
                                simpleError.Message,
                                codeAndRetryableMetadataError.Message,
                            ]
                        },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>()
                    {
                        { "attempts", 3 },
                        { "errorCode", "E-001" },
                        { "retryable", true },
                    },
                }
            ),
            "Errors contributing metadata"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    firstCodeMetadataError,
                    secondCodeMetadataError,
                    thirdCodeMetadataError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",
                            [
                                firstCodeMetadataError.Message,
                                secondCodeMetadataError.Message,
                                thirdCodeMetadataError.Message,
                            ]
                        },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>()
                    {
                        { "errorCode", new object?[] { "E-001", "E-002", "E-003" } },
                        { "attempts", 3 },
                    },
                }
            ),
            "Errors claiming the same metadata key"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("aggregated", traceIdMetadataError, simpleError),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [traceIdMetadataError.Message, simpleError.Message] },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Metadata named traceId not overwriting the trace identifier"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    withFieldValidationError,
                    new AggregateError<IError>(nestedValidationError, nestedSimpleError)
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { withFieldValidationError.FieldName!, [withFieldValidationError.Message] },
                        { nestedValidationError.FieldName!, [nestedValidationError.Message] },
                        { "", [nestedSimpleError.Message] },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Nested aggregated errors keyed by field name"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    simpleError,
                    new AggregateError<IError>(
                        nestedSimpleError,
                        new AggregateError<IError>(nestedCodeMetadataError)
                    )
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            "",
                            [
                                simpleError.Message,
                                nestedSimpleError.Message,
                                nestedCodeMetadataError.Message,
                            ]
                        },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>() { { "errorCode", "E-002" } },
                }
            ),
            "Deeply nested error with metadata"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    firstCodeMetadataError,
                    new AggregateError<IError>(nestedCodeMetadataError)
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        { "", [firstCodeMetadataError.Message, nestedCodeMetadataError.Message] },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>()
                    {
                        { "errorCode", new object?[] { "E-001", "E-002" } },
                    },
                }
            ),
            "Metadata key claimed across nesting"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", new AggregateError<IError>(notFoundError)),
                new ProblemDetails()
                {
                    Detail = "not found",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.NotFound,
                }
            ),
            "Nested single not found error"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    withFieldValidationError,
                    new AggregateError<IError>(
                        withField2ValidationError,
                        secondFieldValidationError,
                        new AggregateError<IError>(
                            deeplyNestedValidationError,
                            simpleError,
                            nestedCodeMetadataError
                        )
                    )
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            withFieldValidationError.FieldName!,
                            [
                                withFieldValidationError.Message,
                                withField2ValidationError.Message,
                                deeplyNestedValidationError.Message,
                            ]
                        },
                        {
                            secondFieldValidationError.FieldName!,
                            [secondFieldValidationError.Message]
                        },
                        { "", [simpleError.Message, nestedCodeMetadataError.Message] },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>() { { "errorCode", "E-002" } },
                }
            ),
            "Same field name across nesting levels"
        );
        yield return TestCase.Of<ErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    "aggregated",
                    fieldWithMetadataValidationError,
                    simpleError
                ),
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>()
                    {
                        {
                            fieldWithMetadataValidationError.FieldName!,
                            [fieldWithMetadataValidationError.Message]
                        },
                        { "", [simpleError.Message] },
                    }
                )
                {
                    Detail = "aggregated",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>() { { "errorCode", "E-100" } },
                }
            ),
            "Validation error carrying both field name and metadata"
        );
    }

    public static IEnumerable<Func<TestDataRow<ErrorCase>>> CreateErrorTestData()
    {
        yield return TestCase.Of<ErrorCase>(
            (
                "/test",
                "my error test",
                new ProblemDetails() { Detail = "my error test", Instance = "/test" }
            ),
            "Error with path"
        );
        yield return TestCase.Of<ErrorCase>(
            (
                "/test2",
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
            ),
            "Error with different path"
        );
        yield return TestCase.Of<ErrorCase>(
            (
                null,
                "my error test 2",
                new ProblemDetails() { Detail = "my error test 2", Instance = null }
            ),
            "Error with null path"
        );
    }

    public static IEnumerable<
        Func<TestDataRow<ErrorWithMetadataCase>>
    > CreateErrorWithMetadataTestData()
    {
        yield return TestCase.Of<ErrorWithMetadataCase>(
            (
                "/test",
                "my metadata error",
                new Dictionary<string, object?>() { { "errorCode", "E-001" } },
                new ProblemDetails()
                {
                    Detail = "my metadata error",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>() { { "errorCode", "E-001" } },
                }
            ),
            "Single metadata entry"
        );
        yield return TestCase.Of<ErrorWithMetadataCase>(
            (
                "/test2",
                "my metadata error 2",
                new Dictionary<string, object?>()
                {
                    { "errorCode", "E-002" },
                    { "attempts", 3 },
                    { "retryable", true },
                },
                new ProblemDetails()
                {
                    Detail = "my metadata error 2",
                    Instance = "/test2",
                    Status = (int)HttpStatusCode.BadRequest,
                    Extensions = new Dictionary<string, object?>()
                    {
                        { "errorCode", "E-002" },
                        { "attempts", 3 },
                        { "retryable", true },
                    },
                }
            ),
            "Multiple metadata entries"
        );
        yield return TestCase.Of<ErrorWithMetadataCase>(
            (
                null,
                "my metadata error 3",
                new Dictionary<string, object?>(),
                new ProblemDetails()
                {
                    Detail = "my metadata error 3",
                    Instance = null,
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Without metadata and path"
        );
        yield return TestCase.Of<ErrorWithMetadataCase>(
            (
                "/test",
                "my metadata error 4",
                new Dictionary<string, object?>() { { "traceId", "spoofed" } },
                new ProblemDetails()
                {
                    Detail = "my metadata error 4",
                    Instance = "/test",
                    Status = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Metadata named traceId not overwriting the trace identifier"
        );
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorWithReasonsTestData))]
    public async Task TestToProblemDetails_WhenErrorWithReasonsGiven_ThenProblemDetailsIsReturned(
        string? path,
        object errorValue,
        ProblemDetails expectedProblemDetails
    )
    {
        AggregateError<IError> error = (AggregateError<IError>)errorValue;
        Guid traceId = Guid.NewGuid();
        Mock<HttpContext> httpContextMock = HttpContextMock.Create(path, traceId.ToString());

        ProblemDetails problemDetails = error.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        using (Assert.Multiple())
        {
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                _ = await Assert
                    .That(validationProblemDetails.Errors)
                    .IsEquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors);
            }

            _ = await Assert
                .That(problemDetails.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"));
            _ = await Assert.That(problemDetails.Status).IsEqualTo(expectedProblemDetails.Status);
            _ = await Assert
                .That(problemDetails.Instance)
                .IsEqualTo(expectedProblemDetails.Instance);
            _ = await Assert
                .That(ErrorExtensionsTest.ExtensionsExceptTraceId(problemDetails))
                .IsEquivalentTo(expectedProblemDetails.Extensions);
            _ = await Assert
                .That(problemDetails.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorTestData))]
    public async Task TestToProblemDetails_WhenErrorGiven_ThenProblemDetailsIsReturned(
        string? path,
        string error,
        ProblemDetails expectedProblemDetails
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpContext> httpContextMock = HttpContextMock.Create(path, traceId.ToString());
        Mock<IError> errorMock = new();
        _ = errorMock.Setup(error => error.Message).Returns(error);

        ProblemDetails validationProblemDetails = errorMock.Object.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(validationProblemDetails.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails.Detail?.Replace("\r\n", "\n"));
            _ = await Assert
                .That(validationProblemDetails.Status)
                .IsEqualTo((int)HttpStatusCode.BadRequest);
            _ = await Assert
                .That(validationProblemDetails.Instance)
                .IsEqualTo(expectedProblemDetails.Instance);
            _ = await Assert
                .That(validationProblemDetails.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorWithMetadataTestData))]
    public async Task TestToProblemDetails_WhenErrorWithMetadataGiven_ThenProblemDetailsWithExtensionsIsReturned(
        string? path,
        string message,
        IReadOnlyDictionary<string, object?> metadata,
        ProblemDetails expectedProblemDetails
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpContext> httpContextMock = HttpContextMock.Create(path, traceId.ToString());
        Error error = new(message, metadata);

        ProblemDetails problemDetails = error.ToProblemDetails(
            path is not null ? httpContextMock.Object : null
        );

        using (Assert.Multiple())
        {
            _ = await Assert.That(problemDetails.Detail).IsEqualTo(expectedProblemDetails.Detail);
            _ = await Assert.That(problemDetails.Status).IsEqualTo(expectedProblemDetails.Status);
            _ = await Assert
                .That(problemDetails.Instance)
                .IsEqualTo(expectedProblemDetails.Instance);

            _ = await Assert
                .That(ErrorExtensionsTest.ExtensionsExceptTraceId(problemDetails))
                .IsEquivalentTo(expectedProblemDetails.Extensions);
            _ = await Assert
                .That(problemDetails.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    private static IDictionary<string, object?> ExtensionsExceptTraceId(
        ProblemDetails problemDetails
    ) =>
        problemDetails
            .Extensions.Where(extension => extension.Key != "traceId")
            .ToDictionary(extension => extension.Key, extension => extension.Value);
}
