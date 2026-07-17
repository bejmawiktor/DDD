using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Utils.Functional;

namespace DDD.Domain.Validation.AspNetCore;

/// <summary>
/// Extension methods that turn functional <c>IResult</c> values into ASP.NET
/// Core <see cref="IActionResult"/> responses, mapping failures to
/// <see cref="ProblemDetails"/> and successes to <c>200 OK</c>.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Converts a value-less result into an action result: a problem-details
    /// response when the result carries an error, otherwise an empty
    /// <c>200 OK</c>.
    /// </summary>
    /// <typeparam name="TError">The error type carried by the result.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="httpContext">The current HTTP context, used for error details. Optional.</param>
    /// <returns>The corresponding action result.</returns>
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

    /// <summary>
    /// Converts a value-carrying result into an action result: a problem-details
    /// response when the result carries an error, otherwise a <c>200 OK</c>
    /// wrapping the value with the requested content types.
    /// </summary>
    /// <typeparam name="TValue">The success value type carried by the result.</typeparam>
    /// <typeparam name="TError">The error type carried by the result.</typeparam>
    /// <param name="result">The result to convert.</param>
    /// <param name="httpContext">The current HTTP context, used for error details. Optional.</param>
    /// <param name="mediaTypes">
    /// The content types to advertise on a successful response. Defaults to
    /// <c>application/json</c> when <see langword="null"/>.
    /// </param>
    /// <returns>The corresponding action result.</returns>
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
