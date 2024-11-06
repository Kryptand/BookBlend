using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.UpdateLibraryPath.Commands;

public sealed class UpdateLibraryPathCommand(Guid id, string path) : IRequest<Result>
{
    public Guid Id { get; set; } = id;
    public string Path { get; set; } = path;
}