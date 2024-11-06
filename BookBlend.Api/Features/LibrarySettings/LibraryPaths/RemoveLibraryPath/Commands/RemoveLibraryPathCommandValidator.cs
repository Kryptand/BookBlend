using FluentValidation;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.RemoveLibraryPath.Commands;

public sealed class RemoveLibraryPathCommandValidator : AbstractValidator<RemoveLibraryPathCommand>
{
    public RemoveLibraryPathCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}