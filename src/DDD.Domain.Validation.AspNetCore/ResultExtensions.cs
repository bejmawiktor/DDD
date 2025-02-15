using DDD.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace DDD.Domain.Validation.AspNetCore;

public static class ResultExtensions
{
    public static IActionResult ToActionResult<TError>(
        this IResult<TError> result,
        HttpContext? httpContext = null
    )
        where TError : IError
    {
        if (result.Error is not null)
        {
            ProblemDetails problemDetails = result.Error.ToProblemDetails(httpContext);

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        return new OkResult();
    }

    public static IActionResult ToActionResult<TException>(
        this IResult<IError<TException>> result,
        HttpContext? httpContext = null
    )
        where TException : Exception
    {
        if (result.Error is not null)
        {
            ProblemDetails problemDetails = result.Error.ToProblemDetails(httpContext);

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        return new OkResult();
    }

    public static IActionResult ToActionResult<TValue, TError>(
        this IResult<TValue, TError> result,
        HttpContext? httpContext = null,
        string[]? mediaTypes = null
    )
        where TError : IError
    {
        if (result.Error is not null)
        {
            ProblemDetails problemDetails = result.Error.ToProblemDetails(httpContext);

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        MediaTypeCollection mediaTypeCollection = new();
        (mediaTypes ?? ["application/json"])
            .ToList()
            .ForEach(mediaType => mediaTypeCollection.Add(mediaType));

        return new OkObjectResult(result.Value) { ContentTypes = mediaTypeCollection };
    }

    public static IActionResult ToActionResult<TValue, TException>(
        this IResult<TValue, IError<TException>> result,
        HttpContext? httpContext = null,
        string[]? mediaTypes = null
    )
        where TException : Exception
    {
        if (result.Error is not null)
        {
            ProblemDetails problemDetails = result.Error.ToProblemDetails(httpContext);

            return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
        }

        MediaTypeCollection mediaTypeCollection = new();
        (mediaTypes ?? ["application/json"])
            .ToList()
            .ForEach(mediaType => mediaTypeCollection.Add(mediaType));

        return new OkObjectResult(result.Value) { ContentTypes = mediaTypeCollection };
    }
}
