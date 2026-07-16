using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Utils.Functional;

namespace DDD.Domain.Validation.AspNetCore;

public static class ResultExtensions
{
    extension<TError>(IResult<TError> result)
        where TError : IError
    {
        public IActionResult ToActionResult(HttpContext? httpContext = null)
        {
            if (result.Error is not null)
            {
                ProblemDetails problemDetails = result.Error.ToProblemDetails(httpContext);

                return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
            }

            return new OkResult();
        }
    }

    extension<TValue, TError>(IResult<TValue, TError> result)
        where TError : IError
    {
        public IActionResult ToActionResult(
            HttpContext? httpContext = null,
            string[]? mediaTypes = null
        )
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
}
