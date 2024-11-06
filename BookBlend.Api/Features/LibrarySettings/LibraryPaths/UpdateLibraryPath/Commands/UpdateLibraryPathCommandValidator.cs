using BookBlend.Api.Database;
using BookBlend.Api.Features.LibrarySettings.LibraryPaths.Shared.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookBlend.Api.Features.LibrarySettings.LibraryPaths.UpdateLibraryPath.Commands;

public sealed class UpdateLibraryPathCommandValidator : AbstractValidator<UpdateLibraryPathCommand>
{
    private readonly AudiobookDbContext _context;
    
    public UpdateLibraryPathCommandValidator(ILibraryPathValidatorService validatorService, AudiobookDbContext context)
    {
        _context = context;
        
        RuleFor(c => c.Id).NotEmpty().WithMessage("Id cannot be null or empty"));
        RuleFor(c => c.Path).NotEmpty().WithMessage("Path cannot be null or empty");
        RuleFor(c => c.Path).Must(validatorService.DirectoryExists).WithMessage("Directory does not exist");
        RuleFor(c => c.Path).Must(validatorService.ValidatePath).WithMessage("Path already exists");
        RuleFor(c => c.Id).MustAsync(async (id, cancellationToken) => await LibraryPathExistsAsync(id, cancellationToken)).WithMessage("Library path not found");
    }

    private async Task<bool> LibraryPathExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.LibraryPaths.AnyAsync(x => x.Id == id, cancellationToken);
    }
}