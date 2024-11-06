using BookBlend.Api.Database;
using BookBlend.Api.Features.LibrarySettings.LibraryPaths.Shared.Services;
using FluentValidation;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands;

public sealed class SetLibrarySettingsCommandValidator : AbstractValidator<SetLibrarySettingsCommand>
{
    private readonly AudiobookDbContext _dbContext;
    private readonly ILibraryPathValidatorService _libraryPathValidatorService;
    
    public SetLibrarySettingsCommandValidator(AudiobookDbContext dbContext, ILibraryPathValidatorService libraryPathValidatorService)
    {
        _dbContext = dbContext;
        _libraryPathValidatorService = libraryPathValidatorService;

        RuleFor(c => c.Paths).NotEmpty().WithMessage("Library paths cannot be null or empty");
        RuleForEach(c => c.Paths).Must(ValidateLibraryPath).WithMessage("Library path does not exist");
    }

    private bool ValidateLibraryPath(string arg)
    {
        return _libraryPathValidatorService.DirectoryExists(arg);
    }
}