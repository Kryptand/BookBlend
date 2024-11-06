using BookBlend.Api.Extensions;
using BookBlend.Api.Features.LibrarySettings.LibraryPaths.RemoveLibraryPath.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.RemoveLibraryPath;

public sealed class RemoveLibraryPathEndpoint:CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/v1/library-settings/paths", async (RemoveLibraryPathCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok();
        });
    }
}