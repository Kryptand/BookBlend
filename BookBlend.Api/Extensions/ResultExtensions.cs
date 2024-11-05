using BookBlend.Api.Shared;

namespace BookBlend.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToProblemDetails(this Result result)
    {
        if (!result.IsFailure)
        {
            throw new InvalidOperationException(
                "The result must be a failure to be converted to a problem details response.");
        }

        return Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                type: "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Error } }
                }
            );
    }
}