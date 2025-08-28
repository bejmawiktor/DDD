using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Utils.Functional;

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

        MediaTypeCollection mediaTypeCollection = [];
        (mediaTypes ?? ["application/json"]).ToList().ForEach(mediaTypeCollection.Add);

        return new OkObjectResult(result.Value) { ContentTypes = mediaTypeCollection };
    }
}
