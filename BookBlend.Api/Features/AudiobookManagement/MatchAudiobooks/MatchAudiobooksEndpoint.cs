using BookBlend.Api.Extensions;
using BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.AudiobookManagement.MatchAudiobooks;

public class MatchAudiobooksEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/match-audiobooks", async (ISender sender) =>
        {
            var command = new MatchAudiobookFilesToAudiobooksCommand();
            
            var result = await sender.Send(command);

            return result.IsFailure ? result.ToProblemDetails() : Results.Ok(result.Value);
        });
    }
}