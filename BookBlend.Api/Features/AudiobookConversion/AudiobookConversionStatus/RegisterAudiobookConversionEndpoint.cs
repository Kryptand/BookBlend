using BookBlend.Api.Extensions;
using BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.AudiobookConversion.AudiobookConversionStatus;

public class RegisterAudiobookConversionEndpoint:CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/audiobook-conversion", async (ISender sender, RegisterAudiobookConversionCommand command) =>
        {
            var result = await sender.Send(command);
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok(result.Value);
        });
    }
}