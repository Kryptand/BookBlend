using BookBlend.Api.Extensions;
using BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands;
using Carter;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings;

public class SetLibrarySettingsEndpoint : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/library-settings", async (SetLibrarySettingsCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);

            return result.IsFailure ? result.ToProblemDetails() : Results.Ok();
        });
    }
}