using BookBlend.Api.Database;
using BookBlend.Api.Entities;
using BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Services;
using BookBlend.Api.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.LibrarySettings.SetLibrarySettings.Commands
{
    public sealed class SetLibrarySettingsCommandHandler : IRequestHandler<SetLibrarySettingsCommand, Result>
    {
        private readonly AudiobookDbContext _dbContext;
        private readonly IValidator<SetLibrarySettingsCommand> _validator;
        private readonly ILibraryPathValidatorService _libraryPathValidatorService;

        public SetLibrarySettingsCommandHandler(
            AudiobookDbContext dbContext,
            IValidator<SetLibrarySettingsCommand> validator,
            ILibraryPathValidatorService libraryPathValidatorService)
        {
            _dbContext = dbContext;
            _validator = validator;
            _libraryPathValidatorService = libraryPathValidatorService;
        }

        public async Task<Result> Handle(SetLibrarySettingsCommand request, CancellationToken cancellationToken)
        {
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure(new Error("SetLibrarySettings.Validation", validationResult.ToString()));
            }
            
            var hasInvalidPaths = request.Paths.Any(path=>!_libraryPathValidatorService.ValidatePath(path));
            
            if (hasInvalidPaths)
            {
                return Result.Failure(new Error("SetLibrarySettings.Validation", "Invalid path"));
            }
            
            var settings = await _dbContext.LibrarySettings.FirstOrDefaultAsync(cancellationToken);

            if (settings == null)
            {
                settings = new Entities.LibrarySettings();
                await _dbContext.LibrarySettings.AddAsync(settings, cancellationToken);
            }

            settings.Paths = request.Paths.Select(path => new LibraryPath { Path = path }).ToList();

            if (request.DefaultLanguage != null)
            {
                settings.DefaultLanguage = request.DefaultLanguage;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return Result.Success();
        }
    }
}