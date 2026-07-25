using Microsoft.AspNetCore.Http;
using Moq;

namespace DDD.Tests.Unit.Domain.Validation.AspNetCore.TestDoubles;

internal static class HttpContextMock
{
    public static Mock<HttpContext> Create(string? path, string traceIdentifier)
    {
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
            .Returns(traceIdentifier);

        return httpContextMock;
    }
}
