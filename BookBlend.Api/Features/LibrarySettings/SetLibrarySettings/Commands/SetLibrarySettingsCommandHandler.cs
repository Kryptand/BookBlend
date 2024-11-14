using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Services;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands
{
    public sealed class SetLibrarySettingsCommandHandler(
        AudiobookDbContext dbContext,
        IValidator<SetLibrarySettingsCommand> validator,
        ILibraryPathValidatorService libraryPathValidatorService)
        : IRequestHandler<SetLibrarySettingsCommand, Result>
    {
        public async Task<Result> Handle(SetLibrarySettingsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure(new Error("SetLibrarySettings.Validation", validationResult.ToString()));
            }
            
            var hasInvalidOutputDirectory = !libraryPathValidatorService.ValidatePath(request.OutputDirectory);
            
            if (hasInvalidOutputDirectory)
            {
                return Result.Failure(new Error("SetLibrarySettings.Validation", "Invalid output directory"));
            }
            
            var invalidPaths = request.Paths.FindAll(path=>!libraryPathValidatorService.ValidatePath(path));
            
            if (invalidPaths.Any())
            {
                return Result.Failure(new Error("SetLibrarySettings.Validation", "Invalid library paths found: " + string.Join(", ", invalidPaths)));
            }
            
            var settings = await dbContext.LibrarySettings.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new Entities.LibrarySettings();
                await dbContext.LibrarySettings.AddAsync(settings, cancellationToken);
            }

            settings.Paths = request.Paths.Select(path => new LibraryPath { Path = path }).ToList();
            
            settings.OutputDirectory = request.OutputDirectory;

            settings.DefaultLanguage = request.DefaultLanguage;

            await dbContext.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}