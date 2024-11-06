using BookBlend.Api.Shared;
using MediatR;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands;

public sealed class SetLibrarySettingsCommand(string defaultLanguage, List<string> paths) : IRequest<Result>
{
    public string? DefaultLanguage { get; } = defaultLanguage;
    public List<string> Paths { get; } = paths;
}