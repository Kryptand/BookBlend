using BookBlend.Api.Extensions;
using BookBlend.Api.Features.FileManagement.FileSystemScanner.Commands;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookBlend.Api.Features.FileManagement.FileSystemScanner;

public class ScanDirectoryForAudiobooksEndpoint: CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/v1/scan-directory", async (ScanDirectoryForAudiobooksCommand command, ISender sender) =>
        {
            var result = await sender.Send(command);
                
            return result.IsFailure ? result.ToProblemDetails() : Results.Ok(result.Value);
        });
    }
}