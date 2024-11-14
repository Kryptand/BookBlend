using BookBlend.Api.Extensions;
using BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Queries;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus;

public class GetConversionStatusEndpoint:CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/v1/audiobook-conversion/{conversionJobId}", async (ISender sender, Guid conversionJobId) =>
        {
            var result = await sender.Send(new GetConversionStatusQuery { ConversionJobId = conversionJobId });
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok(result.Value);
        });
    }
}