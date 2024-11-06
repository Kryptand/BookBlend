using BookBlend.Api.Extensions;
using BookBlend.Api.Features.LibrarySettings.LibraryPaths.AddLibraryPath.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.AddLibraryPath;

public sealed class AddLibraryPathEndpoint:CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/library-settings/paths", async (AddLibraryPathCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok();
        });
    }
}