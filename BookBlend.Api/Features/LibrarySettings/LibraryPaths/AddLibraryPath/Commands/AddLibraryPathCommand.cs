using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.AddLibraryPath.Commands;

public sealed class AddLibraryPathCommand(string path) : IRequest<Result>
{
    public string Path { get; set; } = path;
}