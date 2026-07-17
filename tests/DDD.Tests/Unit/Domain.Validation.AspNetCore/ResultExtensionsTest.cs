using System.Diagnostics;
using System.Net;
using DDD.Domain.Validation.AspNetCore;
using DDD.Tests.Unit.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utils.Functional;
using Utils.Validation;
using OkCase = (string? Path, string[]? MediaTypes, string[]? ExpectedMediaTypes);
using ResultErrorCase = (
    string? Path,
    string Error,
    Microsoft.AspNetCore.Mvc.ObjectResult ExpectedActionResult
);
using ResultErrorWithReasonsCase = (
    string? Path,
    object Error,
    Microsoft.AspNetCore.Mvc.ObjectResult ExpectedActionResult
);

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

public class ResultExtensionsTest
{
    public static IEnumerable<
        Func<TestDataRow<ResultErrorWithReasonsCase>>
    > CreateErrorWithReasonsTestData()
    {
        Error simpleError = new("my error");
        ValidationError withFieldNameValidationError = new("fieldName", "my validation error");
        ValidationError secondWithFieldNameValidationError = new("fieldName", "validation error 2");
        ValidationError thirdWithFieldNameValidationError = new("fieldName2", "validation error 2");
        ValidationError fourthWithFieldNameValidationError = new(
            "fieldName2",
            "validation exception 3"
        );
        ValidationError messageOnlyValidationError = new("validation error without fieldName");
        Error argumentError = new("my argument error");
        NotFoundError notFoundError = new("not found error");
        NotFoundError secondNotFoundError = new("not found exception 2");

        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(simpleError),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>() { { "", [simpleError.Message] } }
                    )
                    {
                        Detail = simpleError.Message,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Single error"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(withFieldNameValidationError),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                withFieldNameValidationError.FieldName!,
                                [withFieldNameValidationError.Message]
                            },
                        }
                    )
                    {
                        Detail = withFieldNameValidationError.Message,
                        Instance = "/test2",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Field name validation error"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                null,
                new AggregateError<IError>(simpleError, argumentError),
                new ObjectResult(
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
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Two errors"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test2",
                new AggregateError<IError>(
                    withFieldNameValidationError,
                    simpleError,
                    argumentError,
                    messageOnlyValidationError,
                    notFoundError
                ),
                new ObjectResult(
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
                            {
                                withFieldNameValidationError.FieldName,
                                [withFieldNameValidationError.Message]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {withFieldNameValidationError.Message}
                          - {simpleError.Message}
                          - {argumentError.Message}
                          - {messageOnlyValidationError.Message}
                          - {notFoundError.Message}
                        """,
                        Instance = "/test2",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple errors with field name"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldNameValidationError,
                    secondWithFieldNameValidationError,
                    simpleError,
                    argumentError
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            { "", [simpleError.Message, argumentError.Message] },
                            {
                                withFieldNameValidationError.FieldName,
                                [
                                    withFieldNameValidationError.Message,
                                    secondWithFieldNameValidationError.Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {withFieldNameValidationError.Message}
                          - {secondWithFieldNameValidationError.Message}
                          - {simpleError.Message}
                          - {argumentError.Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Same field name errors"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldNameValidationError,
                    thirdWithFieldNameValidationError,
                    secondWithFieldNameValidationError,
                    simpleError,
                    argumentError
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            { "", [simpleError.Message, argumentError.Message] },
                            {
                                withFieldNameValidationError.FieldName,
                                [
                                    withFieldNameValidationError.Message,
                                    secondWithFieldNameValidationError.Message,
                                ]
                            },
                            {
                                thirdWithFieldNameValidationError.FieldName,
                                [thirdWithFieldNameValidationError.Message]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {withFieldNameValidationError.Message}
                          - {thirdWithFieldNameValidationError.Message}
                          - {secondWithFieldNameValidationError.Message}
                          - {simpleError.Message}
                          - {argumentError.Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>(
                    withFieldNameValidationError,
                    thirdWithFieldNameValidationError,
                    secondWithFieldNameValidationError,
                    fourthWithFieldNameValidationError,
                    simpleError,
                    argumentError
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            { "", [simpleError.Message, argumentError.Message] },
                            {
                                withFieldNameValidationError.FieldName,
                                [
                                    withFieldNameValidationError.Message,
                                    secondWithFieldNameValidationError.Message,
                                ]
                            },
                            {
                                thirdWithFieldNameValidationError.FieldName,
                                [
                                    thirdWithFieldNameValidationError.Message,
                                    fourthWithFieldNameValidationError.Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {withFieldNameValidationError.Message}
                          - {secondWithFieldNameValidationError.Message}
                          - {thirdWithFieldNameValidationError.Message}
                          - {fourthWithFieldNameValidationError.Message}
                          - {simpleError.Message}
                          - {argumentError.Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple field name errors with multiple messages"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError),
                new ObjectResult(
                    new ProblemDetails()
                    {
                        Detail = "not found",
                        Instance = "/test",
                        Status = (int)HttpStatusCode.NotFound,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                }
            ),
            "Single not found error"
        );
        yield return TestCase.Of<ResultErrorWithReasonsCase>(
            (
                "/test",
                new AggregateError<IError>("not found", notFoundError, secondNotFoundError),
                new ObjectResult(
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
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Multiple not found errors"
        );
    }

    public static IEnumerable<Func<TestDataRow<ResultErrorCase>>> CreateErrorTestData()
    {
        yield return TestCase.Of<ResultErrorCase>(
            (
                "/test",
                "my error test",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test", Instance = "/test" }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Error with path"
        );
        yield return TestCase.Of<ResultErrorCase>(
            (
                "/test2",
                "my error test 2",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Error with different path"
        );
        yield return TestCase.Of<ResultErrorCase>(
            (
                null,
                "my error test 2",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test 2", Instance = null }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ),
            "Error with null path"
        );
    }

    public static IEnumerable<Func<TestDataRow<OkCase>>> OkTestData()
    {
        yield return TestCase.Of<OkCase>(
            (
                null,
                ["application/json", "multipart/form-data"],
                ["application/json", "multipart/form-data"]
            ),
            "Multiple media types without path"
        );
        yield return TestCase.Of<OkCase>(
            ("/path", ["multipart/form-data"], ["multipart/form-data"]),
            "Single media type with path"
        );
        yield return TestCase.Of<OkCase>(
            (null, null, ["application/json"]),
            "No media types defaults to json"
        );
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorWithReasonsTestData))]
    public async Task TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        object errorValue,
        ObjectResult expectedActionResult
    )
    {
        AggregateError<IError> error = (AggregateError<IError>)errorValue;
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<AggregateError<IError>> result = new(error);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(actionResult?.StatusCode)
                .IsEqualTo(expectedActionResult.StatusCode);
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                _ = await Assert
                    .That(validationProblemDetails.Errors)
                    .IsEquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors);
            }

            _ = await Assert
                .That(problemDetails?.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails?.Detail?.Replace("\r\n", "\n"));
            _ = await Assert.That(problemDetails?.Status).IsEqualTo(expectedProblemDetails?.Status);
            _ = await Assert
                .That(problemDetails?.Instance)
                .IsEqualTo(expectedProblemDetails?.Instance);
            _ = await Assert
                .That(problemDetails?.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [Arguments((string?)null)]
    [Arguments("/test")]
    public async Task TestToActionResultWithReason_WhenValueGiven_ThenOkResultIsReturned(
        string? path
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<IError<Exception>> result = new();

        OkResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as OkResult;

        using (Assert.Multiple())
        {
            _ = await Assert.That(actionResult).IsNotNull();
            _ = await Assert.That(actionResult?.StatusCode).IsEqualTo((int)HttpStatusCode.OK);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorTestData))]
    public async Task TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned(
        string? path,
        string error,
        ObjectResult expectedActionResult
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Mock<IError> errorMock = new();
        _ = errorMock.Setup(error => error.Message).Returns(error);
        Result<IError> result = new(errorMock.Object);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(actionResult?.StatusCode)
                .IsEqualTo(expectedActionResult.StatusCode);
            _ = await Assert
                .That(actionResult?.ContentTypes)
                .IsEquivalentTo(expectedActionResult.ContentTypes);
            _ = await Assert.That(problemDetails?.Detail).IsEqualTo(expectedProblemDetails?.Detail);
            _ = await Assert
                .That(problemDetails?.Status)
                .IsEqualTo(expectedActionResult.StatusCode);
            _ = await Assert
                .That(problemDetails?.Instance)
                .IsEqualTo(expectedProblemDetails?.Instance);
            _ = await Assert
                .That(problemDetails?.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [Arguments((string?)null)]
    [Arguments("/test")]
    public async Task TestToActionResult_WhenValueGiven_ThenOkResultIsReturned(string? path)
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<IError> result = new();

        OkResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as OkResult;

        using (Assert.Multiple())
        {
            _ = await Assert.That(actionResult).IsNotNull();
            _ = await Assert.That(actionResult?.StatusCode).IsEqualTo((int)HttpStatusCode.OK);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorWithReasonsTestData))]
    public async Task TestToActionResultWithValue_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        object errorValue,
        ObjectResult expectedActionResult
    )
    {
        AggregateError<IError> error = (AggregateError<IError>)errorValue;
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<object, AggregateError<IError>> result = new(error);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(actionResult?.StatusCode)
                .IsEqualTo(expectedActionResult.StatusCode);
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                _ = await Assert
                    .That(validationProblemDetails.Errors)
                    .IsEquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors);
            }

            _ = await Assert
                .That(problemDetails?.Detail?.Replace("\r\n", "\n"))
                .IsEqualTo(expectedProblemDetails?.Detail?.Replace("\r\n", "\n"));
            _ = await Assert.That(problemDetails?.Status).IsEqualTo(expectedProblemDetails?.Status);
            _ = await Assert
                .That(problemDetails?.Instance)
                .IsEqualTo(expectedProblemDetails?.Instance);
            _ = await Assert
                .That(problemDetails?.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [MethodDataSource(nameof(OkTestData))]
    public async Task TestToActionResultWithValue_WhenValueGiven_ThenOkResultIsReturned(
        string? path,
        string[]? mediaTypes,
        string[]? expectedMediaTypes
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<string, IError> result = new("my result");

        OkObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null, mediaTypes)
            as OkObjectResult;

        using (Assert.Multiple())
        {
            _ = await Assert.That(actionResult).IsNotNull();
            _ = await Assert.That(actionResult?.StatusCode).IsEqualTo((int)HttpStatusCode.OK);
            _ = await Assert.That(actionResult?.Value).IsEqualTo("my result");
            _ = await Assert
                .That(actionResult?.ContentTypes)
                .IsEquivalentTo(expectedMediaTypes ?? []);
        }
    }

    [Test]
    [MethodDataSource(nameof(CreateErrorTestData))]
    public async Task TestToActionResultWithValueWithReasons_WhenErrorGiven_ThenActionResultIsReturned(
        string? path,
        string error,
        ObjectResult expectedActionResult
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Mock<IError> errorMock = new();
        _ = errorMock.Setup(error => error.Message).Returns(error);
        Result<object, IError> result = new(errorMock.Object);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        using (Assert.Multiple())
        {
            _ = await Assert
                .That(actionResult?.StatusCode)
                .IsEqualTo(expectedActionResult.StatusCode);
            _ = await Assert
                .That(actionResult?.ContentTypes)
                .IsEquivalentTo(expectedActionResult.ContentTypes);
            _ = await Assert.That(problemDetails?.Detail).IsEqualTo(expectedProblemDetails?.Detail);
            _ = await Assert.That(problemDetails?.Status).IsEqualTo((int)HttpStatusCode.BadRequest);
            _ = await Assert
                .That(problemDetails?.Instance)
                .IsEqualTo(expectedProblemDetails?.Instance);
            _ = await Assert
                .That(problemDetails?.Extensions["traceId"])
                .IsEqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null));
        }
    }

    [Test]
    [MethodDataSource(nameof(OkTestData))]
    public async Task TestToActionResultWithValueWithReasons_WhenValueGiven_ThenOkResultIsReturned(
        string? path,
        string[]? mediaTypes,
        string[]? expectedMediaTypes
    )
    {
        Guid traceId = Guid.NewGuid();
        Mock<HttpRequest> httpRequestMock = new();
        _ = httpRequestMock
            .Setup(request => request.Path)
            .Returns(PathString.FromUriComponent(path ?? ""));
        Mock<HttpContext> httpContextMock = new();
        _ = httpContextMock
            .Setup(httpContext => httpContext.Request)
            .Returns(httpRequestMock.Object);
        _ = httpContextMock
            .Setup(httpContext => httpContext.TraceIdentifier)
            .Returns(traceId.ToString());
        Result<string, IError<Exception>> result = new("my result");

        OkObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null, mediaTypes)
            as OkObjectResult;

        using (Assert.Multiple())
        {
            _ = await Assert.That(actionResult).IsNotNull();
            _ = await Assert.That(actionResult?.StatusCode).IsEqualTo((int)HttpStatusCode.OK);
            _ = await Assert.That(actionResult?.Value).IsEqualTo("my result");
            _ = await Assert
                .That(actionResult?.ContentTypes)
                .IsEquivalentTo(expectedMediaTypes ?? []);
        }
    }
}
