using Tourism_Api.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Tourism_Api.Abstractions;

public static class ResultExtensions
{
    //public static ObjectResult ToProblem(this Result result, int statusCode)
    //{
    //    if (result.IsSuccess)
    //        throw new InvalidOperationException("Cannot convert success result to a problem");

    //    var problem = Results.Problem(statusCode: statusCode);
    //    var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

    //    problemDetails!.Extensions = new Dictionary<string, object?>
    //    {
    //        {
    //            "errors", new[] { result.Error }
    //        }
    //    };

    //    return new ObjectResult(problemDetails);
    //}
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert success result to a problem");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;

        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors", new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };

        return new ObjectResult(problemDetails);
    }
}