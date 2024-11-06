using BookBlend.Api.Features.LibrarySettings.LibraryPaths.Shared.Services;
using FluentValidation;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.AddLibraryPath.Commands;

public sealed class AddLibraryPathCommandValidator : AbstractValidator<AddLibraryPathCommand>
{
    public AddLibraryPathCommandValidator(ILibraryPathValidatorService validatorService)
    {
        RuleFor(c => c.Path).NotEmpty().WithMessage("Path cannot be null or empty");
        RuleFor(c => c.Path).Must(validatorService.DirectoryExists).WithMessage("Directory does not exist");
        RuleFor(c => c.Path).Must(validatorService.ValidatePath).WithMessage("Path already exists");
    }
}
