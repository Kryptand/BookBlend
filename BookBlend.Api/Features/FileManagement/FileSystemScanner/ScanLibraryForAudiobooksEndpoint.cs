using BookBlend.Api.Extensions;
using BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner;

public class ScanLibraryForAudiobooksEndpoint: CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/scan-library", async (ISender sender) =>
        {
            var result = await sender.Send(new ScanLibraryForAudiobooksCommand());
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok(result.Value);
        });
    }
}