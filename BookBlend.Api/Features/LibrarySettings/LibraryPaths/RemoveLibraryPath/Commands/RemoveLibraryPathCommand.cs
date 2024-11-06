using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.RemoveLibraryPath.Commands;

public sealed class RemoveLibraryPathCommand(Guid id) : IRequest<Result>
{
    public Guid Id { get; set; } = id;
}