using BookBlend.Api.Database;
using FluentValidation;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands;

public sealed class SetLibrarySettingsCommandValidator : AbstractValidator<SetLibrarySettingsCommand>
{
    public SetLibrarySettingsCommandValidator()
    {
        RuleFor(c => c.Paths).NotEmpty().WithMessage("Library paths cannot be null or empty");
    }
}