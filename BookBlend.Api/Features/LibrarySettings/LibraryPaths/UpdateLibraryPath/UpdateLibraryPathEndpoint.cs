using BookBlend.Api.Extensions;
using BookBlend.Api.Features.LibrarySettings.LibraryPaths.UpdateLibraryPath.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.UpdateLibraryPath;

public sealed class UpdateLibraryPathEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/library-settings/path", async (UpdateLibraryPathCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result.IsFailure ? result.ToProblemDetails() : Results.Ok();
        });
    }
}