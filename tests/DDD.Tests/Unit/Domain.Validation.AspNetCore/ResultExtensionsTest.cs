using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DDD.Domain.Validation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Utils.Functional;
using Utils.Validation;

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

[TestFixture]
public class ResultExtensionsTest
{
    public static IEnumerable<TestCaseData> CreateErrorWithReasonsTestData(string testName)
    {
        Error simpleError = new("my error");
        ValidationError withFieldNameValidationError = new("fieldName", "my validation error");
        ValidationError secondWithFieldNameValidationError = new("fieldName", "validation error 2");
        ValidationError thirdWithFieldNameValidationError = new("fieldName2", "validation error 2");
        ValidationError fourthWithFieldNameValidationError =
            new("fieldName2", "validation exception 3");
        ValidationError messageOnlyValidationError = new("validation error without fieldName");
        Error argumentError = new("my argument error");
        NotFoundError notFoundError = new("not found error");
        NotFoundError secondNotFoundError = new("not found exception 2");

        yield return new TestCaseData(
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
        ).SetName($"{testName}(1)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(2)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(3)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(4)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(5)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(6)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(7)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(8)");
        yield return new TestCaseData(
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
        ).SetName($"{testName}(9)");
    }

    public static IEnumerable<TestCaseData> CreateErrorTestData(string testName)
    {
        yield return new TestCaseData(
            "/test",
            "my error test",
            new ObjectResult(new ProblemDetails() { Detail = "my error test", Instance = "/test" })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            }
        ).SetName($"{testName}(1)");
        yield return new TestCaseData(
            "/test2",
            "my error test 2",
            new ObjectResult(
                new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
            )
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            }
        ).SetName($"{testName}(2)");
        yield return new TestCaseData(
            null,
            "my error test 2",
            new ObjectResult(new ProblemDetails() { Detail = "my error test 2", Instance = null })
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
            }
        ).SetName($"{testName}(3)");
    }

    public static IEnumerable<TestCaseData> OkTestData
    {
        get
        {
            yield return new TestCaseData(
                null,
                new string[] { "application/json", "multipart/form-data" },
                new string[] { "application/json", "multipart/form-data" }
            );
            yield return new TestCaseData(
                "/path",
                new string[] { "multipart/form-data" },
                new string[] { "multipart/form-data" }
            );
            yield return new TestCaseData(null, null, new string[] { "application/json" });
        }
    }

    [TestCaseSource(
        nameof(CreateErrorWithReasonsTestData),
        new object[]
        {
            nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned),
        }
    )]
    public void TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        AggregateError<IError> error,
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
        Result<AggregateError<IError>> result = new(error);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        Assert.Multiple(() =>
        {
            Assert.That(actionResult?.StatusCode, Is.EqualTo(expectedActionResult.StatusCode));
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                Assert.That(
                    validationProblemDetails.Errors,
                    Is.EquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors)
                );
            }

            Assert.That(
                problemDetails?.Detail?.Replace("\r\n", "\n"),
                Is.EqualTo(expectedProblemDetails?.Detail?.Replace("\r\n", "\n"))
            );
            Assert.That(problemDetails?.Status, Is.EqualTo(expectedProblemDetails?.Status));
            Assert.That(problemDetails?.Instance, Is.EqualTo(expectedProblemDetails?.Instance));
            Assert.That(
                problemDetails?.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCase(null)]
    [TestCase("/test")]
    public void TestToActionResultWithReason_WhenValueGiven_ThenOkResultIsReturned(string? path)
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult?.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        });
    }

    [TestCaseSource(
        nameof(CreateErrorTestData),
        new object[] { nameof(TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned) }
    )]
    public void TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned(
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult?.StatusCode, Is.EqualTo(expectedActionResult.StatusCode));
            Assert.That(actionResult?.ContentTypes, Is.EqualTo(expectedActionResult.ContentTypes));
            Assert.That(problemDetails?.Detail, Is.EqualTo(expectedProblemDetails?.Detail));
            Assert.That(problemDetails?.Status, Is.EqualTo(expectedActionResult.StatusCode));
            Assert.That(problemDetails?.Instance, Is.EqualTo(expectedProblemDetails?.Instance));
            Assert.That(
                problemDetails?.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCase(null)]
    [TestCase("/test")]
    public void TestToActionResult_WhenValueGiven_ThenOkResultIsReturned(string? path)
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult?.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
        });
    }

    [TestCaseSource(
        nameof(CreateErrorWithReasonsTestData),
        new object[]
        {
            nameof(
                TestToActionResultWithValue_WhenErrorWithReasonsGiven_ThenActionResultIsReturned
            ),
        }
    )]
    public void TestToActionResultWithValue_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        AggregateError<IError> error,
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
        Result<object, AggregateError<IError>> result = new(error);
        ProblemDetails? expectedProblemDetails = expectedActionResult.Value as ProblemDetails;

        ObjectResult? actionResult =
            result.ToActionResult(path is not null ? httpContextMock.Object : null) as ObjectResult;
        ProblemDetails? problemDetails = actionResult?.Value as ProblemDetails;

        Assert.Multiple(() =>
        {
            Assert.That(actionResult?.StatusCode, Is.EqualTo(expectedActionResult.StatusCode));
            if (problemDetails is ValidationProblemDetails validationProblemDetails)
            {
                Assert.That(
                    validationProblemDetails.Errors,
                    Is.EquivalentTo((expectedProblemDetails as ValidationProblemDetails)!.Errors)
                );
            }

            Assert.That(
                problemDetails?.Detail?.Replace("\r\n", "\n"),
                Is.EqualTo(expectedProblemDetails?.Detail?.Replace("\r\n", "\n"))
            );
            Assert.That(problemDetails?.Status, Is.EqualTo(expectedProblemDetails?.Status));
            Assert.That(problemDetails?.Instance, Is.EqualTo(expectedProblemDetails?.Instance));
            Assert.That(
                problemDetails?.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCaseSource(nameof(OkTestData))]
    public void TestToActionResultWithValue_WhenValueGiven_ThenOkResultIsReturned(
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult?.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(actionResult?.Value, Is.EqualTo("my result"));
            Assert.That(actionResult?.ContentTypes, Is.EquivalentTo(expectedMediaTypes ?? []));
        });
    }

    [TestCaseSource(
        nameof(CreateErrorTestData),
        new object[]
        {
            nameof(
                TestToActionResultWithValueWithReasons_WhenErrorGiven_ThenActionResultIsReturned
            ),
        }
    )]
    public void TestToActionResultWithValueWithReasons_WhenErrorGiven_ThenActionResultIsReturned(
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult?.StatusCode, Is.EqualTo(expectedActionResult.StatusCode));
            Assert.That(actionResult?.ContentTypes, Is.EqualTo(expectedActionResult.ContentTypes));
            Assert.That(problemDetails?.Detail, Is.EqualTo(expectedProblemDetails?.Detail));
            Assert.That(problemDetails?.Status, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(problemDetails?.Instance, Is.EqualTo(expectedProblemDetails?.Instance));
            Assert.That(
                problemDetails?.Extensions["traceId"],
                Is.EqualTo(Activity.Current?.Id ?? (path is not null ? traceId.ToString() : null))
            );
        });
    }

    [TestCaseSource(nameof(OkTestData))]
    public void TestToActionResultWithValueWithReasons_WhenValueGiven_ThenOkResultIsReturned(
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

        Assert.Multiple(() =>
        {
            Assert.That(actionResult, Is.Not.Null);
            Assert.That(actionResult?.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
            Assert.That(actionResult?.Value, Is.EqualTo("my result"));
            Assert.That(actionResult?.ContentTypes, Is.EquivalentTo(expectedMediaTypes ?? []));
        });
    }
}
