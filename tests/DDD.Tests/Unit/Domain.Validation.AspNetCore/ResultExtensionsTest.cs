using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using DDD.Domain.Utils;
using DDD.Domain.Validation;
using DDD.Domain.Validation.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore;

[TestFixture]
public class ResultExtensionsTest
{
    public static IEnumerable<TestCaseData> ErrorWithReasonsTestData
    {
        get
        {
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>([new Exception("my exception")]),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            { "", [new Exception("my exception").Message] },
                        }
                    )
                    {
                        Detail = new Exception("my exception").Message,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(1)"
            );
            yield return new TestCaseData(
                "/test2",
                new ValidationError<Exception>(
                    [new ValidationException("fieldName", "my validation exception")]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "fieldName",

                                [
                                    new ValidationException(
                                        "fieldName",
                                        "my validation exception"
                                    ).Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = new ValidationException(
                            "fieldName",
                            "my validation exception"
                        ).Message,
                        Instance = "/test2",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(2)"
            );
            yield return new TestCaseData(
                null,
                new ValidationError<Exception>(
                    [new Exception("my exception"), new ArgumentException("my argument exception")]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new Exception("my exception").Message,
                                    new ArgumentException("my argument exception").Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {new Exception("my exception").Message}
                          - {new ArgumentException("my argument exception").Message}
                        """,
                        Instance = null,
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(3)"
            );
            yield return new TestCaseData(
                "/test2",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                        new ValidationException("validation exception without fieldName"),
                        new NotFoundException("not found exception"),
                    ]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new Exception("my exception").Message,
                                    new ArgumentException("my argument exception").Message,
                                    new ValidationException(
                                        "validation exception without fieldName"
                                    ).Message,
                                    new NotFoundException("not found exception").Message,
                                ]
                            },
                            {
                                "fieldName",

                                [
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception"
                                    ).Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {new ValidationException("fieldName", "validation exception").Message}
                          - {new Exception("my exception").Message}
                          - {new ArgumentException("my argument exception").Message}
                          - {new ValidationException(
                            "validation exception without fieldName"
                        ).Message}
                          - {new NotFoundException("not found exception").Message}
                        """,
                        Instance = "/test2",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(4)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new Exception("my exception").Message,
                                    new ArgumentException("my argument exception").Message,
                                ]
                            },
                            {
                                "fieldName",

                                [
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception"
                                    ).Message,
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception 2"
                                    ).Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {new ValidationException("fieldName", "validation exception").Message}
                          - {new ValidationException("fieldName", "validation exception 2").Message}
                          - {new Exception("my exception").Message}
                          - {new ArgumentException("my argument exception").Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(5)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName2", "validation exception 2"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new Exception("my exception").Message,
                                    new ArgumentException("my argument exception").Message,
                                ]
                            },
                            {
                                "fieldName",

                                [
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception"
                                    ).Message,
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception 2"
                                    ).Message,
                                ]
                            },
                            {
                                "fieldName2",

                                [
                                    new ValidationException(
                                        "fieldName2",
                                        "validation exception 2"
                                    ).Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {new ValidationException("fieldName", "validation exception").Message}
                          - {new ValidationException(
                            "fieldName2",
                            "validation exception 2"
                        ).Message}
                          - {new ValidationException("fieldName", "validation exception 2").Message}
                          - {new Exception("my exception").Message}
                          - {new ArgumentException("my argument exception").Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(6)"
            );
            yield return new TestCaseData(
                "/test",
                new ValidationError<Exception>(
                    [
                        new ValidationException("fieldName", "validation exception"),
                        new ValidationException("fieldName2", "validation exception 2"),
                        new ValidationException("fieldName", "validation exception 2"),
                        new ValidationException("fieldName2", "validation exception 3"),
                        new Exception("my exception"),
                        new ArgumentException("my argument exception"),
                    ]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new Exception("my exception").Message,
                                    new ArgumentException("my argument exception").Message,
                                ]
                            },
                            {
                                "fieldName",

                                [
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception"
                                    ).Message,
                                    new ValidationException(
                                        "fieldName",
                                        "validation exception 2"
                                    ).Message,
                                ]
                            },
                            {
                                "fieldName2",

                                [
                                    new ValidationException(
                                        "fieldName2",
                                        "validation exception 2"
                                    ).Message,
                                    new ValidationException(
                                        "fieldName2",
                                        "validation exception 3"
                                    ).Message,
                                ]
                            },
                        }
                    )
                    {
                        Detail = $"""
                        Multiple errors found:
                          - {new ValidationException("fieldName", "validation exception").Message}
                          - {new ValidationException(
                            "fieldName2",
                            "validation exception 2"
                        ).Message}
                          - {new ValidationException("fieldName", "validation exception 2").Message}
                          - {new ValidationException(
                            "fieldName2",
                            "validation exception 3"
                        ).Message}
                          - {new Exception("my exception").Message}
                          - {new ArgumentException("my argument exception").Message}
                        """,
                        Instance = "/test",
                        Status = (int)HttpStatusCode.BadRequest,
                    }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(7)"
            );
            yield return new TestCaseData(
                "/test",
                new Error<Exception>("not found", [new NotFoundException("not found exception")]),
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
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(8)"
            );
            yield return new TestCaseData(
                "/test",
                new Error<Exception>(
                    "not found",
                    [
                        new NotFoundException("not found exception"),
                        new NotFoundException("not found exception 2"),
                    ]
                ),
                new ObjectResult(
                    new ValidationProblemDetails(
                        new Dictionary<string, string[]>()
                        {
                            {
                                "",

                                [
                                    new NotFoundException("not found exception").Message,
                                    new NotFoundException("not found exception 2").Message,
                                ]
                            },
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
            ).SetName(
                $"{nameof(TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned)}(9)"
            );
        }
    }

    public static IEnumerable<TestCaseData> ErrorTestData
    {
        get
        {
            yield return new TestCaseData(
                "/test",
                "my error test",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test", Instance = "/test" }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName($"{nameof(TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned)}(1)");
            yield return new TestCaseData(
                "/test2",
                "my error test 2",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test 2", Instance = "/test2" }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName($"{nameof(TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned)}(2)");
            yield return new TestCaseData(
                null,
                "my error test 2",
                new ObjectResult(
                    new ProblemDetails() { Detail = "my error test 2", Instance = null }
                )
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                }
            ).SetName($"{nameof(TestToActionResult_WhenErrorGiven_ThenActionResultIsReturned)}(3)");
        }
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

    [TestCaseSource(nameof(ErrorWithReasonsTestData))]
    public void TestToActionResult_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        IError<Exception> error,
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
        Result<IError<Exception>> result = new(error);
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

            Assert.That(problemDetails?.Detail, Is.EqualTo(expectedProblemDetails?.Detail));
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

    [TestCaseSource(nameof(ErrorTestData))]
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

    [TestCaseSource(nameof(ErrorWithReasonsTestData))]
    public void TestToActionResultWithValue_WhenErrorWithReasonsGiven_ThenActionResultIsReturned(
        string? path,
        IError<Exception> error,
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
        Result<object, IError<Exception>> result = new(error);
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

            Assert.That(problemDetails?.Detail, Is.EqualTo(expectedProblemDetails?.Detail));
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

    [TestCaseSource(nameof(ErrorTestData))]
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
